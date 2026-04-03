
namespace qvzhongren.Model.DoctorBaseModel
{
    /// <summary>
    /// 患者会诊申请主表
    ///</summary>
    [SugarTable("MET_CONSULTATION_MASTER")]
    public class MetConsultationMaster : BaseAuditEntity
    {


        /// <summary>
        /// 备  注:会诊id
        /// 默认值: seq_consultation_applyno
        ///</summary>
        [SugarColumn(ColumnName = "CNSL_NO", IsPrimaryKey = true)]
        public string CnslNo { get; set; } = null!;

        /// <summary>
        /// 备  注:患者id
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PATIENT_NO")]
        public string? PatientNo { get; set; }

        /// <summary>
        /// 备  注:挂号科室
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DEPT_CODE")]
        public string? DeptCode { get; set; }

        /// <summary>
        /// 备  注:就诊流水号/ 住院流水号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CLINIC_NO")]
        public string? ClinicNo { get; set; }

        /// <summary>
        /// 备  注:申请时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "APPLY_DATE")]
        public DateTime? ApplyDate { get; set; }

        /// <summary>
        /// 备  注:会诊类型
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CONSULTATION_TYPE")]
        public string? ConsultationType { get; set; }

        /// <summary>
        /// 备  注:会诊说明
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CONSULTATION_EXPLAIN")]
        public string? ConsultationExplain { get; set; }

        /// <summary>
        /// 备  注:开始时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "BEGIN_DATE")]
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 备  注:结束时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "END_DATE")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 备  注:会诊状态 0申请状态
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CONSULTATION_STATUS")]
        public string? ConsultationStatus { get; set; }

        /// <summary>
        /// 备  注:会诊申请医生
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "APPLY_DOCTOR")]
        public string? ApplyDoctor { get; set; }

        /// <summary>
        /// 备  注:完成时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "FINISH_DATE")]
        public DateTime? FinishDate { get; set; }


    }

}