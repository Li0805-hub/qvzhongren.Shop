using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Application.Dtos.DoctorBase
{
    public class FinIprAllergyResponseDto
    {

        /// <summary>
        /// 备  注:患者id
        /// 默认值:
        ///</summary>
        public string PatientNo { get; set; }

        /// <summary>
        /// 备  注:药品编码
        /// 默认值:
        ///</summary>
        public string DrugCode { get; set; } 

        /// <summary>
        /// 备  注:药品名称
        /// 默认值:
        ///</summary>
        public string DrugName { get; set; }

        /// <summary>
        /// 备  注:皮试结果 1：阴性 2：阳性
        /// 默认值:
        ///</summary>
        public string SkinResult { get; set; }
    }
}
