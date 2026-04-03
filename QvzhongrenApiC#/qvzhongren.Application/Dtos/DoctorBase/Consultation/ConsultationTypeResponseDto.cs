using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Application.Dtos.DoctorBase.Consultation
{
    public class ConsultationTypeResponseDto
    {  /// <summary>
       /// 备  注:会诊类型编码
       /// 默认值:
       ///</summary>
        public string TypeCode { get; set; } 

        /// <summary>
        /// 备  注:会诊类型
        /// 默认值:
        ///</summary>
        public string? TypeName { get; set; }

        /// <summary>
        /// 备  注:使用范围
        /// 默认值:
        ///</summary>
        public string? UseFlag { get; set; }

        /// <summary>
        /// 备  注:收费项目编码
        /// 默认值:
        ///</summary>
        public string? ChargeItemCode { get; set; }

        /// <summary>
        /// 备  注:医嘱项目编码
        /// 默认值:
        ///</summary>
        public string? OrderItemCode { get; set; }

        /// <summary>
        /// 备  注:顺序号
        /// 默认值:
        ///</summary>
        public int? SerialNo { get; set; }
    }
}
