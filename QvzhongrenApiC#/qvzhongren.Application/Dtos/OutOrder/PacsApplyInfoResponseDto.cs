using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Application.Dtos.OutOrder
{
    /// <summary>
    /// PACS申请单信息响应Dto
    /// </summary>
    public class PacsApplyInfoResponseDto
    {
        /// <summary>
        /// 备  注:申请单信息表唯一标识
        ///</summary>
        public string ApplyNo { get; set; } = null!;

        /// <summary>
        /// 备  注:患者ID
        ///</summary>
        public string PatientNo { get; set; }

        /// <summary>
        /// 备  注:就诊流水  门诊流水号。住院次数
        ///</summary>
        public string ClinicNo { get; set; }

        /// <summary>
        /// 备  注:患者来源  0 门诊 1住院 2体检
        ///</summary>
        public string PatientSource { get; set; }

        /// <summary>
        /// 备  注:检查类型ID
        ///</summary>
        public string ApplyTypeCode { get; set; }

        /// <summary>
        /// 备  注:检查类型名称
        ///</summary>
        public string ApplyTypeName { get; set; }

        /// <summary>
        /// 备  注:诊断
        /// 默认值:
        ///</summary>
        public string Diagnosis { get; set; }

        /// <summary>
        /// 备  注:主诉及病史
        ///</summary>
        public string ChiefComplaint { get; set; }

        /// <summary>
        /// 备  注:费用
        /// 默认值:
        ///</summary>
        public decimal? Cost { get; set; }

        /// <summary>
        /// 备  注:收费状态  0开立 1收费
        /// 默认值:
        ///</summary>
        public string ChargeFlag { get; set; }

        /// <summary>
        /// 备  注:申请医生
        /// 默认值:
        ///</summary>
        public string ApplyDoctor { get; set; }

        /// <summary>
        /// 备  注:申请时间
        /// 默认值:
        ///</summary>
        public DateTime? ApplyDate { get; set; }

        /// <summary>
        /// 备  注:执行科室
        /// 默认值:
        ///</summary>
        public string ExecDept { get; set; }

        /// <summary>
        /// 备  注:检查状态 0开立 1确定 2确认报告 3作废
        /// 默认值:
        ///</summary>
        public string ExamStatus { get; set; }

        /// <summary>
        /// 备  注:申请科室
        /// 默认值:
        ///</summary>
        public string ApplyDept { get; set; }

        /// <summary>
        /// 备  注:加急标志 1是 0否
        /// 默认值:
        ///</summary>
        public string UrgentFlag { get; set; }

        /// <summary>
        /// 备  注:检查方法
        /// 默认值:
        ///</summary>
        public string ExamMethodCode { get; set; }

        /// <summary>
        /// 备  注:检查方法
        /// 默认值:
        ///</summary>
        public string ExamMethodName { get; set; }

        /// <summary>
        /// 备  注:检查部位
        /// 默认值:
        ///</summary>
        public string ExamPartCode { get; set; }

        /// <summary>
        /// 备  注:检查部位
        /// 默认值:
        ///</summary>
        public string ExamPartName { get; set; }

        /// <summary>
        /// 备  注:检查具体描述
        /// 默认值:
        ///</summary>
        public string ExamDescribeCode { get; set; }

        /// <summary>
        /// 备  注:检查具体描述
        /// 默认值:
        ///</summary>
        public string ExamDescribeName { get; set; }

        /// <summary>
        /// 备  注:医嘱序列号
        /// 默认值:
        ///</summary>
        public string MoOrder { get; set; } = null!;

        /// <summary>
        /// 备  注:项目编码(医嘱)
        /// 默认值:
        ///</summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 备  注:项目名称(医嘱)
        /// 默认值:
        ///</summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 备  注:数量
        /// 默认值:
        ///</summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 备  注:报告人
        /// 默认值:
        ///</summary>
        public string ReporterCode { get; set; }

        /// <summary>
        /// 备  注:报告时间
        /// 默认值:
        ///</summary>
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// 备  注:确认人
        /// 默认值:
        ///</summary>
        public string ConfirmedCode { get; set; }

        /// <summary>
        /// 备  注:确认时间
        /// 默认值:
        ///</summary>
        public DateTime? ConfirmedDate { get; set; }

        /// <summary>
        /// 备  注:打印标志
        /// 默认值:
        ///</summary>
        public string PrintFlag { get; set; }

        /// <summary>
        /// 备  注:院区编码
        /// 默认值:
        ///</summary>
        public string HosUnitCode { get; set; }

        /// <summary>
        /// 处方号
        /// </summary>
        public string PrescNo { get; set; }

        /// <summary>
        /// 现病史
        /// </summary>
        public string Illness { get; set; }

        /// <summary>
        /// 目的
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// 体征
        /// </summary>
        public string PhysicalSigns { get; set; }
    }
}
