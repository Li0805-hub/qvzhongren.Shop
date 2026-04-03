namespace qvzhongren.Shop.Application.Dtos
{
    /// <summary>
    /// 商品评价响应DTO
    /// </summary>
    public class ReviewResponseDto
    {
        /// <summary>
        /// 评价ID
        /// </summary>
        public string ReviewId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 评分（1-5星）
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 评价图片（JSON数组）
        /// </summary>
        public string? Images { get; set; }

        /// <summary>
        /// 创建人编码
        /// </summary>
        public string CreateCode { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 用户名称（关联查询）
        /// </summary>
        public string? UserName { get; set; }
    }

    /// <summary>
    /// 商品评价创建DTO
    /// </summary>
    public class ReviewCreateDto
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 评分（1-5星）
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 评价图片（JSON数组）
        /// </summary>
        public string? Images { get; set; }
    }
}
