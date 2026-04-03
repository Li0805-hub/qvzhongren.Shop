using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Repository.SqlSugar;
using qvzhongren.Shared.Common;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;
using SqlSugar;

namespace qvzhongren.Shop.Application.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderService : BaseService
    {
        private readonly IBaseRepository<ShopOrder> _orderRepository;
        private readonly IBaseRepository<ShopOrderItem> _orderItemRepository;
        private readonly IBaseRepository<ShopProduct> _productRepository;
        private readonly IBaseRepository<ShopAddress> _addressRepository;
        private readonly IBaseRepository<ShopCart> _cartRepository;
        private readonly IMapper _mapper;
        private readonly ISqlSugarClient _db;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderService(
            IBaseRepository<ShopOrder> orderRepository,
            IBaseRepository<ShopOrderItem> orderItemRepository,
            IBaseRepository<ShopProduct> productRepository,
            IBaseRepository<ShopAddress> addressRepository,
            IBaseRepository<ShopCart> cartRepository,
            IMapper mapper,
            ISqlSugarClient db,
            ICurrentUserService currentUserService)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _addressRepository = addressRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _db = db;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="dto">订单创建信息</param>
        /// <returns>创建结果</returns>
        [HttpPost("Create")]
        public async Task<ResultDto<string>> CreateAsync([FromBody] OrderCreateDto dto)
        {
            try
            {
                var userId = _currentUserService.User?.UserCode;
                if (string.IsNullOrEmpty(userId))
                {
                    return ResultDto<string>.Error("未获取到当前用户信息", 401);
                }

                if (dto.Items == null || dto.Items.Count == 0)
                {
                    return ResultDto<string>.BadRequest("订单商品不能为空");
                }

                // 获取收货地址
                var address = await _addressRepository.GetByIdAsync(dto.AddressId);
                if (address == null)
                {
                    return ResultDto<string>.Error("收货地址不存在");
                }

                // 校验商品库存并获取商品信息
                var productIds = dto.Items.Select(x => x.ProductId).ToList();
                var products = await _productRepository.GetListAsync(x => productIds.Contains(x.ProductId));

                decimal totalAmount = 0;
                var orderItems = new List<ShopOrderItem>();
                var orderId = Guid.NewGuid().ToString("N");

                foreach (var item in dto.Items)
                {
                    var product = products.FirstOrDefault(x => x.ProductId == item.ProductId);
                    if (product == null)
                    {
                        return ResultDto<string>.Error($"商品不存在: {item.ProductId}");
                    }

                    if (product.Stock < item.Quantity)
                    {
                        return ResultDto<string>.Error($"商品【{product.ProductName}】库存不足，当前库存: {product.Stock}");
                    }

                    var subtotal = product.Price * item.Quantity;
                    totalAmount += subtotal;

                    orderItems.Add(new ShopOrderItem
                    {
                        ItemId = Guid.NewGuid().ToString("N"),
                        OrderId = orderId,
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductImage = product.MainImage,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        Subtotal = subtotal,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    });
                }

                // 生成订单编号
                var random = new Random();
                var orderNo = DateTime.Now.ToString("yyyyMMddHHmmss") + random.Next(1000, 9999).ToString();

                // 构建订单
                var order = new ShopOrder
                {
                    OrderId = orderId,
                    OrderNo = orderNo,
                    UserId = userId,
                    TotalAmount = totalAmount,
                    Status = "0",
                    ReceiverName = address.ReceiverName,
                    ReceiverPhone = address.ReceiverPhone,
                    ReceiverAddress = $"{address.Province}{address.City}{address.District}{address.DetailAddress}",
                    Remark = dto.Remark,
                    CreateCode = userId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                // 事务处理：创建订单、订单明细、扣减库存、清除购物车
                try
                {
                    _db.Ado.BeginTran();

                    // 插入订单
                    await _db.Insertable(order).ExecuteCommandAsync();

                    // 插入订单明细
                    await _db.Insertable(orderItems).ExecuteCommandAsync();

                    // 扣减库存、增加销量
                    foreach (var item in dto.Items)
                    {
                        await _db.Updateable<ShopProduct>()
                            .SetColumns(x => x.Stock == x.Stock - item.Quantity)
                            .SetColumns(x => x.Sales == x.Sales + item.Quantity)
                            .Where(x => x.ProductId == item.ProductId)
                            .ExecuteCommandAsync();
                    }

                    // 清除购物车中已下单的商品
                    await _db.Deleteable<ShopCart>()
                        .Where(x => x.UserId == userId && productIds.Contains(x.ProductId))
                        .ExecuteCommandAsync();

                    _db.Ado.CommitTran();

                    return ResultDto<string>.Success(orderId, "下单成功");
                }
                catch (Exception)
                {
                    _db.Ado.RollbackTran();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ResultDto<string>.Error($"下单失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 分页查询订单
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>分页订单列表</returns>
        [HttpPost("GetPage")]
        public async Task<ResultDto<ListPageResultDto<OrderResponseDto>>> GetPageAsync([FromBody] OrderQueryDto query)
        {
            try
            {
                var totalCount = new RefAsync<int>();
                var list = await _db.Queryable<ShopOrder>()
                    .WhereIF(!string.IsNullOrEmpty(query.Status), x => x.Status == query.Status)
                    .WhereIF(!string.IsNullOrEmpty(query.UserId), x => x.UserId == query.UserId)
                    .OrderBy(x => x.CreateDate, OrderByType.Desc)
                    .ToPageListAsync(query.PageIndex, query.PageSize, totalCount);

                var dtoList = _mapper.Map<List<OrderResponseDto>>(list);

                // 查询所有订单的明细
                if (dtoList.Count > 0)
                {
                    var orderIds = dtoList.Select(x => x.OrderId).ToList();
                    var allItems = await _db.Queryable<ShopOrderItem>()
                        .Where(x => orderIds.Contains(x.OrderId))
                        .ToListAsync();

                    foreach (var order in dtoList)
                    {
                        order.Items = _mapper.Map<List<OrderItemDto>>(
                            allItems.Where(x => x.OrderId == order.OrderId).ToList());
                    }
                }

                var result = new ListPageResultDto<OrderResponseDto>
                {
                    TotalCount = totalCount.Value,
                    PageIndex = query.PageIndex,
                    PageSize = query.PageSize,
                    Values = dtoList
                };

                return ResultDto<ListPageResultDto<OrderResponseDto>>.Success(result, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<ListPageResultDto<OrderResponseDto>>.Error($"获取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据ID获取订单详情（含明细）
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>订单详情</returns>
        [HttpPost("GetById")]
        public async Task<ResultDto<OrderResponseDto>> GetByIdAsync([FromBody] string orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(orderId))
                {
                    return ResultDto<OrderResponseDto>.BadRequest("订单ID不能为空");
                }

                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    return ResultDto<OrderResponseDto>.Error("订单不存在");
                }

                var dto = _mapper.Map<OrderResponseDto>(order);

                // 查询订单明细
                var items = await _orderItemRepository.GetListAsync(x => x.OrderId == orderId);
                dto.Items = _mapper.Map<List<OrderItemDto>>(items);

                return ResultDto<OrderResponseDto>.Success(dto, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<OrderResponseDto>.Error($"获取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 取消订单（恢复库存）
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>取消结果</returns>
        [HttpPost("Cancel")]
        public async Task<ResultDto<bool>> CancelAsync([FromBody] string orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(orderId))
                {
                    return ResultDto<bool>.BadRequest("订单ID不能为空");
                }

                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    return ResultDto<bool>.Error("订单不存在");
                }

                if (order.Status != "0" && order.Status != "1")
                {
                    return ResultDto<bool>.Error("当前订单状态不允许取消");
                }

                // 获取订单明细用于恢复库存
                var items = await _orderItemRepository.GetListAsync(x => x.OrderId == orderId);

                try
                {
                    _db.Ado.BeginTran();

                    // 更新订单状态为已取消
                    await _db.Updateable<ShopOrder>()
                        .SetColumns(x => x.Status == "-1")
                        .SetColumns(x => x.UpdateDate == DateTime.Now)
                        .Where(x => x.OrderId == orderId)
                        .ExecuteCommandAsync();

                    // 恢复库存、减少销量
                    foreach (var item in items)
                    {
                        await _db.Updateable<ShopProduct>()
                            .SetColumns(x => x.Stock == x.Stock + item.Quantity)
                            .SetColumns(x => x.Sales == x.Sales - item.Quantity)
                            .Where(x => x.ProductId == item.ProductId)
                            .ExecuteCommandAsync();
                    }

                    _db.Ado.CommitTran();

                    return ResultDto<bool>.Success(true, "取消成功");
                }
                catch (Exception)
                {
                    _db.Ado.RollbackTran();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"取消失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新订单状态（管理员：发货2、完成3）
        /// </summary>
        /// <param name="dto">状态更新信息</param>
        /// <returns>更新结果</returns>
        [HttpPost("UpdateStatus")]
        public async Task<ResultDto<bool>> UpdateStatusAsync([FromBody] OrderUpdateStatusDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.OrderId))
                {
                    return ResultDto<bool>.BadRequest("订单ID不能为空");
                }

                var order = await _orderRepository.GetByIdAsync(dto.OrderId);
                if (order == null)
                {
                    return ResultDto<bool>.Error("订单不存在");
                }

                order.Status = dto.Status;
                order.UpdateDate = DateTime.Now;

                // 根据状态设置对应时间
                switch (dto.Status)
                {
                    case "2":
                        order.ShipTime = DateTime.Now;
                        break;
                    case "3":
                        order.CompleteTime = DateTime.Now;
                        break;
                }

                var result = await _orderRepository.UpdateAsync(order);
                return result > 0
                    ? ResultDto<bool>.Success(true, "更新成功")
                    : ResultDto<bool>.Error("更新失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"更新失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 沙盒支付（模拟付款，将订单状态改为已付款/待发货）
        /// </summary>
        [HttpPost("Pay")]
        public async Task<ResultDto<bool>> PayAsync([FromBody] string orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(orderId))
                    return ResultDto<bool>.BadRequest("订单ID不能为空");

                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    return ResultDto<bool>.Error("订单不存在");
                if (order.Status != "0")
                    return ResultDto<bool>.Error("该订单不是待付款状态");

                order.Status = "1"; // 待发货
                order.PayTime = DateTime.Now;
                order.UpdateDate = DateTime.Now;

                var result = await _orderRepository.UpdateAsync(order);
                return result > 0
                    ? ResultDto<bool>.Success(true, "支付成功")
                    : ResultDto<bool>.Error("支付失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"支付失败: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 订单状态更新DTO
    /// </summary>
    public class OrderUpdateStatusDto
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string Status { get; set; }
    }
}
