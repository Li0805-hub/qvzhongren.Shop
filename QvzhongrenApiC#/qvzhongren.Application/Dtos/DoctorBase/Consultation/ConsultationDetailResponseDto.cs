using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Application.Dtos.DoctorBase.Consultation
{
    /// <summary>
    /// 会诊详情
    /// </summary>
    public class ConsultationDetailResponseDto
    {
        /// <summary>
        /// 备  注:会诊id
        ///</summary>
        public string CnslNo { get; set; } 

        /// <summary>
        /// 备  注:就诊流水号
        ///</summary>
        public string ClinicNo { get; set; }

        /// <summary>
        /// 备  注:患者id
        ///</summary>
        public string PatientNo { get; set; }

        /// <summary>
        /// 备  注:会诊子序号
        ///</summary>
        public string SubNo { get; set; } 

        /// <summary>
        /// 备  注:会诊科室
        ///</summary>
        public string ConsultationDept { get; set; }

        /// <summary>
        /// 备  注:会诊医生
        ///</summary>
        public string ConsultationDoctor { get; set; }

        /// <summary>
        /// 备  注:会诊时间
        ///</summary>
        public DateTime? CommitDateTime { get; set; }

        /// <summary>
        /// 备  注:会诊意见
        ///</summary>
        public string ConsultationIdea { get; set; }

        /// <summary>
        /// 备  注:会诊意见提交标志
        ///</summary>
        public string CommitFlag { get; set; }

    }
}
