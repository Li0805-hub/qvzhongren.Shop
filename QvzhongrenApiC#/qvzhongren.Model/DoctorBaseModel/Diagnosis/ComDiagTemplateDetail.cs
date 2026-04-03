
namespace qvzhongren.Model.DoctorBaseModel
{
    /// <summary>
    /// 科室和个人常用诊断
    ///</summary>
    [SugarTable("COM_DIAG_TEMPLATE_DETAIL")]
    public class ComDiagTemplateDetail : BaseAuditEntity
    {


        /// <summary>
        /// 备  注:icd10码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_CODE", IsPrimaryKey = true)]
        public string DiagCode { get; set; } = null!;

        /// <summary>
        /// 备  注:操作员
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "OPER_CODE")]
        public string? OperCode { get; set; }

        /// <summary>
        /// 备  注:操作日期
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "OPER_DATE")]
        public DateTime? OperDate { get; set; }

        /// <summary>
        /// 备  注:中西医诊断标志 1中医  0西医
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_CLASS")]
        public string? DiagClass { get; set; }

        /// <summary>
        /// 备  注:模板编码
        /// 默认值: seq_com_diag_templatecode
        ///</summary>
        [SugarColumn(ColumnName = "TEMPLATE_CODE", IsPrimaryKey = true)]
        public string TemplateCode { get; set; } = null!;

        /// <summary>
        /// 备  注:诊断名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_NAME")]
        public string? DiagName { get; set; }

        /// <summary>
        /// 备  注:顺序号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "SERIAL_NO")]
        public decimal? SerialNo { get; set; }


    }

}