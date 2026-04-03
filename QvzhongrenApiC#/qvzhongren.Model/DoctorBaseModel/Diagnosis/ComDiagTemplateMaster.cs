namespace qvzhongren.Model.DoctorBaseModel
{
    /// <summary>
    /// 科室和个人常用诊断模板主表
    ///</summary>
    [SugarTable("COM_DIAG_TEMPLATE_MASTER")]
    public class ComDiagTemplateMaster : BaseAuditEntity
    {


        /// <summary>
        /// 备  注:模板编码
        /// 默认值: seq_com_diag_templatecode
        ///</summary>
        [SugarColumn(ColumnName = "TEMPLATE_CODE", IsPrimaryKey = true)]
        public string TemplateCode { get; set; } = null!;

        /// <summary>
        /// 备  注:模板名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "TEMPLATE_NAME")]
        public string? TemplateName { get; set; }

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
        /// 备  注:科室编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DEPT_CODE")]
        public string? DeptCode { get; set; }

        /// <summary>
        /// 备  注:医生编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DOCTOR_CODE")]
        public string? DoctorCode { get; set; }


    }

}