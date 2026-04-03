namespace qvzhongren.Shop.Application.Dtos
{
    /// <summary>
    /// 购物车响应DTO
    /// </summary>
    public class CartResponseDto
    {
        /// <summary>
        /// 购物车ID
        /// </summary>
        public string CartId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品名称（关联查询）
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// 商品图片（关联查询）
        /// </summary>
        public string? ProductImage { get; set; }

        /// <summary>
        /// 商品价格（关联查询）
        /// </summary>
        public decimal? Price { get; set; }
    }

    /// <summary>
    /// 购物车添加DTO
    /// </summary>
    public class CartAddDto
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; } = 1;
    }
}
