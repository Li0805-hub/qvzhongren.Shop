using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Model
{
    /// <summary>
    /// 过敏药物信息
    ///</summary>
    [SugarTable("FIN_IPR_ALLERGY")]
    public class FinIprAllergy:BaseAuditEntity
    {

        /// <summary>
        /// 备  注:住院流水号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PATIENT_NO", IsPrimaryKey = true)]
        public string PatientNo { get; set; } 

        /// <summary>
        /// 备  注:药品编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DRUG_CODE", IsPrimaryKey = true)]
        public string DrugCode { get; set; } 

        /// <summary>
        /// 备  注:药品名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DRUG_NAME")]
        public string DrugName { get; set; }

        /// <summary>
        /// 备  注:皮试结果 1：阴性 2：阳性
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "SKIN_RESULT")]
        public string SkinResult { get; set; }



    }

}
