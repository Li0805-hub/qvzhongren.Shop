using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Shop.Model
{
    /// <summary>
    /// 商品评价
    /// </summary>
    [SugarTable("SHOP_REVIEW")]
    public class ShopReview : BaseAuditEntity
    {
        /// <summary>
        /// 评价ID
        /// </summary>
        [SugarColumn(ColumnName = "REVIEW_ID", IsPrimaryKey = true)]
        public string ReviewId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [SugarColumn(ColumnName = "ORDER_ID")]
        public string OrderId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [SugarColumn(ColumnName = "PRODUCT_ID")]
        public string ProductId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(ColumnName = "USER_ID")]
        public string UserId { get; set; }

        /// <summary>
        /// 评分（1-5星）
        /// </summary>
        [SugarColumn(ColumnName = "RATING")]
        public int Rating { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        [SugarColumn(ColumnName = "CONTENT", IsNullable = true, ColumnDataType = "text")]
        public string? Content { get; set; }

        /// <summary>
        /// 评价图片（JSON数组）
        /// </summary>
        [SugarColumn(ColumnName = "IMAGES", IsNullable = true, ColumnDataType = "text")]
        public string? Images { get; set; }
    }
}
