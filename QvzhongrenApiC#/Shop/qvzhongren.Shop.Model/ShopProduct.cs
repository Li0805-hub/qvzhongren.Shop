using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Shop.Model
{
    /// <summary>
    /// 商品
    /// </summary>
    [SugarTable("SHOP_PRODUCT")]
    public class ShopProduct : BaseAuditEntity
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        [SugarColumn(ColumnName = "PRODUCT_ID", IsPrimaryKey = true)]
        public string ProductId { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        [SugarColumn(ColumnName = "CATEGORY_ID")]
        public string CategoryId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [SugarColumn(ColumnName = "PRODUCT_NAME")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [SugarColumn(ColumnName = "DESCRIPTION", IsNullable = true, ColumnDataType = "text")]
        public string? Description { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [SugarColumn(ColumnName = "PRICE")]
        public decimal Price { get; set; }

        /// <summary>
        /// 原价/市场价
        /// </summary>
        [SugarColumn(ColumnName = "ORIGINAL_PRICE", IsNullable = true)]
        public decimal? OriginalPrice { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [SugarColumn(ColumnName = "STOCK")]
        public int Stock { get; set; }

        /// <summary>
        /// 总销量
        /// </summary>
        [SugarColumn(ColumnName = "SALES")]
        public int Sales { get; set; } = 0;

        /// <summary>
        /// 商品主图URL
        /// </summary>
        [SugarColumn(ColumnName = "MAIN_IMAGE", IsNullable = true)]
        public string? MainImage { get; set; }

        /// <summary>
        /// 商品图片集（JSON数组）
        /// </summary>
        [SugarColumn(ColumnName = "IMAGES", IsNullable = true, ColumnDataType = "text")]
        public string? Images { get; set; }

        /// <summary>
        /// 状态（1=上架，0=下架）
        /// </summary>
        [SugarColumn(ColumnName = "STATUS")]
        public string Status { get; set; } = "1";

        /// <summary>
        /// 排序号
        /// </summary>
        [SugarColumn(ColumnName = "SORT_NO", IsNullable = true)]
        public int? SortNo { get; set; }
    }
}
