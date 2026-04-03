namespace qvzhongren.Shop.Application.Dtos
{
    /// <summary>
    /// 商品响应DTO
    /// </summary>
    public class ProductResponseDto
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 原价/市场价
        /// </summary>
        public decimal? OriginalPrice { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 总销量
        /// </summary>
        public int Sales { get; set; }

        /// <summary>
        /// 商品主图URL
        /// </summary>
        public string? MainImage { get; set; }

        /// <summary>
        /// 商品图片集（JSON数组）
        /// </summary>
        public string? Images { get; set; }

        /// <summary>
        /// 状态（1=上架，0=下架）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? SortNo { get; set; }

        /// <summary>
        /// 创建人编码
        /// </summary>
        public string CreateCode { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改人编码
        /// </summary>
        public string UpdateCode { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }

    /// <summary>
    /// 商品创建/更新DTO
    /// </summary>
    public class ProductCreateDto
    {
        /// <summary>
        /// 商品ID（更新时传入）
        /// </summary>
        public string? ProductId { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 原价/市场价
        /// </summary>
        public decimal? OriginalPrice { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 商品主图URL
        /// </summary>
        public string? MainImage { get; set; }

        /// <summary>
        /// 商品图片集（JSON数组）
        /// </summary>
        public string? Images { get; set; }

        /// <summary>
        /// 状态（1=上架，0=下架）
        /// </summary>
        public string Status { get; set; } = "1";

        /// <summary>
        /// 排序号
        /// </summary>
        public int? SortNo { get; set; }
    }

    /// <summary>
    /// 商品查询DTO
    /// </summary>
    public class ProductQueryDto
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string? Status { get; set; }

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
