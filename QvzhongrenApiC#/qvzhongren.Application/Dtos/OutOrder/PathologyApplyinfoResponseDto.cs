using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Application.Dtos.OutOrder
{
    /// <summary>
    /// 病理申请单信息表
    /// </summary>
    public class PathologyApplyinfoResponseDto
    {

        /// <summary>
        /// 备  注:申请单信息表唯一标识
        /// 默认值:
        ///</summary>
        public string ApplyNo { get; set; }

        /// <summary>
        /// 备  注:住院流水号
        /// 默认值:
        ///</summary>
        public string InpatientNo { get; set; }

        /// <summary>
        /// 备  注:申请日期
        /// 默认值:
        ///</summary>
        public DateTime? ApplyDate { get; set; }

        /// <summary>
        /// 备  注:申请医生
        /// 默认值:
        ///</summary>
        public string ApplyDoctor { get; set; }

        /// <summary>
        /// 备  注:病理项目编码
        /// 默认值:
        ///</summary>
        public string ItemCode { get; set; } 

        /// <summary>
        /// 备  注:病理项目名称
        /// 默认值:
        ///</summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 备  注:送检时间
        /// 默认值:
        ///</summary>
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// 备  注:送检人
        /// 默认值:
        ///</summary>
        public string SenderCode { get; set; }

        /// <summary>
        /// 备  注:执行科室
        /// 默认值:
        ///</summary>
        public string ExecDeptCode { get; set; }

        /// <summary>
        /// 备  注:术中标本标志
        /// 默认值:
        ///</summary>
        public string IntraoperativeFlag { get; set; }

        /// <summary>
        /// 备  注:病历摘要
        /// 默认值:
        ///</summary>
        public string Abstracthistory { get; set; }

        /// <summary>
        /// 备  注:诊断
        /// 默认值:
        ///</summary>
        public string Diagnosis { get; set; }

        /// <summary>
        /// 备  注:末次月经
        /// 默认值:
        ///</summary>
        public DateTime? LastMenses { get; set; }

        /// <summary>
        /// 备  注:持续天数
        /// 默认值:
        ///</summary>
        public int ContinuousDay { get; set; }

        /// <summary>
        /// 备  注:手术名称及术中所见
        /// 默认值:
        ///</summary>
        public string IntraoperativeMemo { get; set; }

        /// <summary>
        /// 备  注:传染病
        /// 默认值:
        ///</summary>
        public string Infection { get; set; }

        /// <summary>
        /// 备  注:备注
        /// 默认值:
        ///</summary>
        public string Memo { get; set; }

        /// <summary>
        /// 备  注:申请状态 0申请 1审核 2执行 3作废
        /// 默认值:
        ///</summary>
        public string Status { get; set; }

        /// <summary>
        /// 备  注:医嘱号
        /// 默认值:
        ///</summary>
        public string MoOrder { get; set; }

        /// <summary>
        /// 备  注:申请单类型编码
        /// 默认值:
        ///</summary>
        public string ApplytypeCode { get; set; }

        /// <summary>
        /// 备  注:申请单类型名称
        /// 默认值:
        ///</summary>
        public string ApplytypeName { get; set; }

        /// <summary>
        /// 备  注:患者类型 0门诊 1住院
        /// 默认值:
        ///</summary>
        public int PatientType { get; set; }

        /// <summary>
        /// 备  注:年龄
        /// 默认值:
        ///</summary>
        public string Age { get; set; }

        /// <summary>
        /// 备  注:检查目的
        /// 默认值:
        ///</summary>
        public string Checkobj { get; set; }

        /// <summary>
        /// 备  注:妊娠史
        /// 默认值:
        ///</summary>
        public string Prehistory { get; set; }

        /// <summary>
        /// 备  注:月经史
        /// 默认值:
        ///</summary>
        public string Menhistory { get; set; }

        /// <summary>
        /// 备  注:申请科室代码
        /// 默认值:
        ///</summary>
        public string ApplyDeptcode { get; set; }

        /// <summary>
        /// 备  注:申请科室名称
        /// 默认值:
        ///</summary>
        public string ApplyDeptname { get; set; }

        /// <summary>
        /// 备  注:数量
        /// 默认值:
        ///</summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 备  注:病理登记状态
        /// 默认值:
        ///</summary>
        public string CheckState { get; set; }

        /// <summary>
        /// 备  注:
        /// 默认值:
        ///</summary>
        public string UploadFlag { get; set; }

        /// <summary>
        /// 备  注:就诊卡号
        /// 默认值:
        ///</summary>
        public string PatientNo { get; set; }

        /// <summary>
        /// 处方号
        /// </summary>
        public string PrescNo { get; set; }
    }
}
