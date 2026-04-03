namespace qvzhongren.Model.DoctorBaseModel
{
    /// <summary>
    /// 患者诊断表
    ///</summary>
    [SugarTable("MET_COM_DIAGNOSIS")]
    public class MetComDiagnosis : BaseAuditEntity
    {
        /// <summary>
        /// 备  注:患者ID 
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true, OracleSequenceName = "seq_met_com_diagnosis")]
        public string Id { get; set; }

        /// <summary>
        /// 备  注:患者ID 
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PATIENT_NO")]
        public string PatientNo { get; set; } 

        /// <summary>
        /// 备  注:就诊流水号。 门诊挂号流水/住院流水号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CLINIC_NO")]
        public string ClinicNo { get; set; } 

        /// <summary>
        /// 备  注:诊断类型   
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_TYPE")]
        public string DiagType { get; set; } 

        /// <summary>
        /// 备  注:诊断类别  中西医类别
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_CLASS")]
        public string? DiagClass { get; set; }

        /// <summary>
        /// 备  注:诊断序号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_NO")]
        public short? DiagNo { get; set; }

        /// <summary>
        /// 备  注:诊断编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_CODE")]
        public string DiagCode { get; set; } 

        /// <summary>
        /// 备  注:诊断名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_NAME")]
        public string? DiagName { get; set; }

        /// <summary>
        /// 备  注:诊断描述
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_DESC")]
        public string? DiagDesc { get; set; }

        /// <summary>
        /// 备  注:诊断科室
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DEPT_CODE")]
        public string? DeptCode { get; set; }

        /// <summary>
        /// 备  注:下达医生
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_DOCTOR_CODE")]
        public string? DiagDoctorCode { get; set; }

        /// <summary>
        /// 备  注:诊断时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_DATE")]
        public DateTime? DiagDate { get; set; }

        /// <summary>
        /// 备  注:操作时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "OPER_DATE")]
        public DateTime? OperDate { get; set; }

        /// <summary>
        /// 备  注:上级医生审核/签名
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_CHARGE_CODE")]
        public string? DiagChargeCode { get; set; }

        /// <summary>
        /// 备  注:上级医生签名时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CHARGE_SIGN_DATE")]
        public DateTime? ChargeSignDate { get; set; }

        /// <summary>
        /// 备  注:上级医生操作时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CHARGE_OPER_DATE")]
        public DateTime? ChargeOperDate { get; set; }

        /// <summary>
        /// 备  注:主任医生审核/签名
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DIAG_CHIEF_CODE")]
        public string? DiagChiefCode { get; set; }

        /// <summary>
        /// 备  注:主任医生签名时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CHIEF_SIGN_DATE")]
        public DateTime? ChiefSignDate { get; set; }

        /// <summary>
        /// 备  注:主任医生操作时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CHIEF_OPER_DATE")]
        public DateTime? ChiefOperDate { get; set; }

        /// <summary>
        /// 备  注:签名等级   1经治 2上级(主治)  3主任 
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "LEVEL_FLAG")]
        public string? LevelFlag { get; set; }

        /// <summary>
        /// 主诊断标志
        /// </summary>
        [SugarColumn(ColumnName = "MAIN_FLAG")]
        public string MainFlag { get; set; }

        /// <summary>
        /// 疑似诊断标志
        /// </summary>
        [SugarColumn(ColumnName = "SUSPECTED_FLAG")]
        public string SuspectedFlag { get; set; }
    }

}