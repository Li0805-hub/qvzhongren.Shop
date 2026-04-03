namespace qvzhongren.Shop.Application.Dtos
{
    /// <summary>
    /// 订单响应DTO
    /// </summary>
    public class OrderResponseDto
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ReceiverPhone { get; set; }

        /// <summary>
        /// 完整收货地址
        /// </summary>
        public string ReceiverAddress { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? ShipTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 创建人编码
        /// </summary>
        public string CreateCode { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 订单明细列表
        /// </summary>
        public List<OrderItemDto> Items { get; set; } = new();
    }

    /// <summary>
    /// 订单明细DTO
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// 明细ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 商品名称（快照）
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片（快照）
        /// </summary>
        public string? ProductImage { get; set; }

        /// <summary>
        /// 购买时单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 小计
        /// </summary>
        public decimal Subtotal { get; set; }
    }

    /// <summary>
    /// 订单创建DTO
    /// </summary>
    public class OrderCreateDto
    {
        /// <summary>
        /// 收货地址ID
        /// </summary>
        public string AddressId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 订单商品列表
        /// </summary>
        public List<OrderCreateItemDto> Items { get; set; } = new();
    }

    /// <summary>
    /// 订单创建商品明细DTO
    /// </summary>
    public class OrderCreateItemDto
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 订单查询DTO
    /// </summary>
    public class OrderQueryDto
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}
