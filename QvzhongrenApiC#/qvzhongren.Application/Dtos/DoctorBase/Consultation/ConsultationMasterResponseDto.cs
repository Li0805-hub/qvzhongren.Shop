using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Application.Dtos.DoctorBase.Consultation
{
    /// <summary>
    /// 会诊主表
    /// </summary>
    public class ConsultationMasterResponseDto
    {
        /// <summary>
        /// 备  注:会诊id
        ///</summary>
        public string CnslNo { get; set; }

        /// <summary>
        /// 备  注:患者id
        ///</summary>
        public string PatientNo { get; set; }

        /// <summary>
        /// 备  注:挂号科室
        ///</summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// 备  注:就诊流水号/ 住院流水号
        ///</summary>
        public string ClinicNo { get; set; }

        /// <summary>
        /// 备  注:申请时间
        ///</summary>
        public DateTime? ApplyDate { get; set; }

        /// <summary>
        /// 备  注:会诊类型
        ///</summary>
        public string ConsultationType { get; set; }

        /// <summary>
        /// 备  注:会诊说明
        ///</summary>
        public string ConsultationExplain { get; set; }

        /// <summary>
        /// 备  注:开始时间
        ///</summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 备  注:结束时间
        ///</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 备  注:会诊状态 0申请状态
        ///</summary>
        public string ConsultationStatus { get; set; }

        /// <summary>
        /// 备  注:会诊申请医生
        ///</summary>
        public string ApplyDoctor { get; set; }

        /// <summary>
        /// 备  注:完成时间
        ///</summary>
        public DateTime? FinishDate { get; set; }
    }
}
