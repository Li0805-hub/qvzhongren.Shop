namespace qvzhongren.Model.DoctorBaseModel
{
    /// <summary>
    /// 患者会诊明细表
    ///</summary>
    [SugarTable("MET_CONSULTATION_DETAIL")]
    public class MetConsultationDetail : BaseAuditEntity
    {
        /// <summary>
        /// 备  注:会诊id
        /// 默认值: seq_consultation_applyno
        ///</summary>
        [SugarColumn(ColumnName = "CNSL_NO", IsPrimaryKey = true)]
        public string CnslNo { get; set; } = null!;

        /// <summary>
        /// 备  注:就诊流水号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CLINIC_NO")]
        public string? ClinicNo { get; set; }

        /// <summary>
        /// 备  注:患者id
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PATIENT_NO")]
        public string? PatientNo { get; set; }

        /// <summary>
        /// 备  注:会诊子序号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "SUB_NO", IsPrimaryKey = true)]
        public string SubNo { get; set; } = null!;

        /// <summary>
        /// 备  注:会诊科室
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CONSULTATION_DEPT")]
        public string? ConsultationDept { get; set; }

        /// <summary>
        /// 备  注:会诊医生
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CONSULTATION_DOCTOR")]
        public string? ConsultationDoctor { get; set; }

        /// <summary>
        /// 备  注:会诊时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "COMMIT_DATE_TIME")]
        public DateTime? CommitDateTime { get; set; }

        /// <summary>
        /// 备  注:会诊意见
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CONSULTATION_IDEA")]
        public string? ConsultationIdea { get; set; }

        /// <summary>
        /// 备  注:会诊意见提交标志
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "COMMIT_FLAG")]
        public string? CommitFlag { get; set; }


    }

}