using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Shop.Model
{
    /// <summary>
    /// 订单
    /// </summary>
    [SugarTable("SHOP_ORDER")]
    public class ShopOrder : BaseAuditEntity
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        [SugarColumn(ColumnName = "ORDER_ID", IsPrimaryKey = true)]
        public string OrderId { get; set; }

        /// <summary>
        /// 订单编号（如"202604021234"）
        /// </summary>
        [SugarColumn(ColumnName = "ORDER_NO")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(ColumnName = "USER_ID")]
        public string UserId { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        [SugarColumn(ColumnName = "TOTAL_AMOUNT")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 订单状态（0=待付款，1=已付款，2=已发货，3=已完成，-1=已取消）
        /// </summary>
        [SugarColumn(ColumnName = "STATUS")]
        public string Status { get; set; } = "0";

        /// <summary>
        /// 收货人姓名（地址快照）
        /// </summary>
        [SugarColumn(ColumnName = "RECEIVER_NAME")]
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        [SugarColumn(ColumnName = "RECEIVER_PHONE")]
        public string ReceiverPhone { get; set; }

        /// <summary>
        /// 完整收货地址（地址快照）
        /// </summary>
        [SugarColumn(ColumnName = "RECEIVER_ADDRESS")]
        public string ReceiverAddress { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "REMARK", IsNullable = true)]
        public string? Remark { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        [SugarColumn(ColumnName = "PAY_TIME", IsNullable = true)]
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        [SugarColumn(ColumnName = "SHIP_TIME", IsNullable = true)]
        public DateTime? ShipTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [SugarColumn(ColumnName = "COMPLETE_TIME", IsNullable = true)]
        public DateTime? CompleteTime { get; set; }
    }
}
