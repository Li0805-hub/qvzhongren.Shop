using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Shop.Model
{
    /// <summary>
    /// 购物车
    /// </summary>
    [SugarTable("SHOP_CART")]
    public class ShopCart : BaseAuditEntity
    {
        /// <summary>
        /// 购物车ID
        /// </summary>
        [SugarColumn(ColumnName = "CART_ID", IsPrimaryKey = true)]
        public string CartId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(ColumnName = "USER_ID")]
        public string UserId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [SugarColumn(ColumnName = "PRODUCT_ID")]
        public string ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [SugarColumn(ColumnName = "QUANTITY")]
        public int Quantity { get; set; } = 1;
    }
}
