using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Shop.Model
{
    /// <summary>
    /// 收货地址
    /// </summary>
    [SugarTable("SHOP_ADDRESS")]
    public class ShopAddress : BaseAuditEntity
    {
        /// <summary>
        /// 地址ID
        /// </summary>
        [SugarColumn(ColumnName = "ADDRESS_ID", IsPrimaryKey = true)]
        public string AddressId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(ColumnName = "USER_ID")]
        public string UserId { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        [SugarColumn(ColumnName = "RECEIVER_NAME")]
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        [SugarColumn(ColumnName = "RECEIVER_PHONE")]
        public string ReceiverPhone { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [SugarColumn(ColumnName = "PROVINCE")]
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [SugarColumn(ColumnName = "CITY")]
        public string City { get; set; }

        /// <summary>
        /// 区/县
        /// </summary>
        [SugarColumn(ColumnName = "DISTRICT")]
        public string District { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [SugarColumn(ColumnName = "DETAIL_ADDRESS")]
        public string DetailAddress { get; set; }

        /// <summary>
        /// 是否默认地址（1=默认，0=非默认）
        /// </summary>
        [SugarColumn(ColumnName = "IS_DEFAULT")]
        public string IsDefault { get; set; } = "0";
    }
}
