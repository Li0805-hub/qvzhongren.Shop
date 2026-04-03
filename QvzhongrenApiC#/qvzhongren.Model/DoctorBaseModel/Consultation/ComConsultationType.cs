
namespace qvzhongren.Model.DoctorBaseModel
{
    /// <summary>
    /// 会诊类型表
    ///</summary>
    [SugarTable("COM_CONSULTATION_TYPE")]
    public class ComConsultationType : BaseAuditEntity
    {
        /// <summary>
        /// 备  注:会诊类型编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "TYPE_CODE", IsPrimaryKey = true)]
        public string TypeCode { get; set; } = null!;

        /// <summary>
        /// 备  注:会诊类型
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "TYPE_NAME")]
        public string? TypeName { get; set; }

        /// <summary>
        /// 备  注:使用范围
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "USE_FLAG")]
        public string? UseFlag { get; set; }

        /// <summary>
        /// 备  注:收费项目编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CHARGE_ITEM_CODE")]
        public string? ChargeItemCode { get; set; }

        /// <summary>
        /// 备  注:医嘱项目编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ORDER_ITEM_CODE")]
        public string? OrderItemCode { get; set; }

        /// <summary>
        /// 备  注:顺序号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "SERIAL_NO")]
        public int? SerialNo { get; set; }



    }

}