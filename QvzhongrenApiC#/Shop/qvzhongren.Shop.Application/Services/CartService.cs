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
    /// 购物车服务
    /// </summary>
    public class CartService : BaseService
    {
        private readonly IBaseRepository<ShopCart> _repository;
        private readonly IMapper _mapper;
        private readonly ISqlSugarClient _db;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CartService(
            IBaseRepository<ShopCart> repository,
            IMapper mapper,
            ISqlSugarClient db,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _db = db;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// 获取用户购物车列表（关联商品信息）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>购物车列表</returns>
        [HttpPost("GetList")]
        public async Task<ResultDto<List<CartResponseDto>>> GetListAsync([FromBody] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return ResultDto<List<CartResponseDto>>.BadRequest("用户ID不能为空");
                }

                var list = await _db.Queryable<ShopCart, ShopProduct>((c, p) =>
                        new JoinQueryInfos(JoinType.Left, c.ProductId == p.ProductId))
                    .Where((c, p) => c.UserId == userId)
                    .Select((c, p) => new CartResponseDto
                    {
                        CartId = c.CartId,
                        UserId = c.UserId,
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        ProductName = p.ProductName,
                        ProductImage = p.MainImage,
                        Price = p.Price
                    })
                    .ToListAsync();

                return ResultDto<List<CartResponseDto>>.Success(list, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<List<CartResponseDto>>.Error($"获取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加商品到购物车（如果已存在则累加数量）
        /// </summary>
        /// <param name="dto">购物车添加信息</param>
        /// <returns>添加结果</returns>
        [HttpPost("Add")]
        public async Task<ResultDto<bool>> AddAsync([FromBody] CartAddDto dto)
        {
            try
            {
                var userId = _currentUserService.User?.UserCode;
                if (string.IsNullOrEmpty(userId))
                {
                    return ResultDto<bool>.Error("未获取到当前用户信息", 401);
                }

                // 检查商品是否已在购物车中
                var existing = await _repository.GetFirstAsync(x => x.UserId == userId && x.ProductId == dto.ProductId);
                if (existing != null)
                {
                    // 已存在，累加数量
                    existing.Quantity += dto.Quantity;
                    existing.UpdateDate = DateTime.Now;
                    var updateResult = await _repository.UpdateAsync(existing);
                    return updateResult > 0
                        ? ResultDto<bool>.Success(true, "添加成功")
                        : ResultDto<bool>.Error("添加失败");
                }

                // 不存在，新增
                var entity = new ShopCart
                {
                    CartId = Guid.NewGuid().ToString("N"),
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                var result = await _repository.InsertAsync(entity);
                return result > 0
                    ? ResultDto<bool>.Success(true, "添加成功")
                    : ResultDto<bool>.Error("添加失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"添加失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        /// <param name="cartId">购物车ID</param>
        /// <param name="quantity">数量</param>
        /// <returns>更新结果</returns>
        [HttpPost("UpdateQuantity")]
        public async Task<ResultDto<bool>> UpdateQuantityAsync([FromBody] CartUpdateQuantityDto dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(dto.CartId);
                if (entity == null)
                {
                    return ResultDto<bool>.Error("购物车记录不存在");
                }

                entity.Quantity = dto.Quantity;
                entity.UpdateDate = DateTime.Now;

                var result = await _repository.UpdateAsync(entity);
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
        /// 删除购物车商品
        /// </summary>
        /// <param name="cartId">购物车ID</param>
        /// <returns>删除结果</returns>
        [HttpPost("Remove")]
        public async Task<ResultDto<bool>> RemoveAsync([FromBody] string cartId)
        {
            try
            {
                if (string.IsNullOrEmpty(cartId))
                {
                    return ResultDto<bool>.BadRequest("购物车ID不能为空");
                }

                var result = await _repository.DeleteByIdAsync(cartId);
                return result > 0
                    ? ResultDto<bool>.Success(true, "删除成功")
                    : ResultDto<bool>.Error("删除失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"删除失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 清空用户购物车
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>清空结果</returns>
        [HttpPost("Clear")]
        public async Task<ResultDto<bool>> ClearAsync([FromBody] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return ResultDto<bool>.BadRequest("用户ID不能为空");
                }

                var result = await _repository.DeleteAsync(x => x.UserId == userId);
                return ResultDto<bool>.Success(true, "清空成功");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"清空失败: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 购物车更新数量DTO
    /// </summary>
    public class CartUpdateQuantityDto
    {
        /// <summary>
        /// 购物车ID
        /// </summary>
        public string CartId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
