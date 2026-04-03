using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Shop.Model
{
    /// <summary>
    /// 订单明细
    /// </summary>
    [SugarTable("SHOP_ORDER_ITEM")]
    public class ShopOrderItem : BaseAuditEntity
    {
        /// <summary>
        /// 明细ID
        /// </summary>
        [SugarColumn(ColumnName = "ITEM_ID", IsPrimaryKey = true)]
        public string ItemId { get; set; }

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
        /// 商品名称（快照）
        /// </summary>
        [SugarColumn(ColumnName = "PRODUCT_NAME")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片（快照）
        /// </summary>
        [SugarColumn(ColumnName = "PRODUCT_IMAGE", IsNullable = true)]
        public string? ProductImage { get; set; }

        /// <summary>
        /// 购买时单价
        /// </summary>
        [SugarColumn(ColumnName = "PRICE")]
        public decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [SugarColumn(ColumnName = "QUANTITY")]
        public int Quantity { get; set; }

        /// <summary>
        /// 小计
        /// </summary>
        [SugarColumn(ColumnName = "SUBTOTAL")]
        public decimal Subtotal { get; set; }
    }
}
