using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qvzhongren.Application.Dtos.Register
{
    /// <summary>
    /// 患者挂号信息主索引DTO
    /// </summary>
    public class FinOprRegisterResponseDto
    {
        /// <summary>
        /// 门诊就诊号
        /// </summary>
        public string ClinicCode { get; set; } = null!;

        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string PatientNo { get; set; } = null!;

        /// <summary>
        /// 挂号日期
        /// </summary>
        public DateTime ClinicDate { get; set; }

        /// <summary>
        /// 午别
        /// </summary>
        public string TimeInterval { get; set; }

        /// <summary>
        /// 结算类别号
        /// </summary>
        public string PaykindCode { get; set; }

        /// <summary>
        /// 结算类别名称
        /// </summary>
        public string PaykindName { get; set; }

        /// <summary>
        /// 合同号
        /// </summary>
        public string PactCode { get; set; }

        /// <summary>
        /// 合同单位名称
        /// </summary>
        public string PactName { get; set; }

        /// <summary>
        /// 医疗证号
        /// </summary>
        public string McardNo { get; set; }

        /// <summary>
        /// 挂号级别
        /// </summary>
        public string ReglevelCode { get; set; }

        /// <summary>
        /// 挂号级别名称
        /// </summary>
        public string ReglevelName { get; set; }

        /// <summary>
        /// 科室号
        /// </summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 看诊序号
        /// </summary>
        public int Seeno { get; set; }

        /// <summary>
        /// 医师代号
        /// </summary>
        public string DoctorCode { get; set; }

        /// <summary>
        /// 医师姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 挂号收费标志 1是/0否
        /// </summary>
        public string RegchargeFlag { get; set; }

        /// <summary>
        /// 发票号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 0现场挂号/1预约挂号/2特诊挂号 3体检号
        /// </summary>
        public string Ynbook { get; set; }

        /// <summary>
        /// 1初诊/0复诊
        /// </summary>
        public string Ynfr { get; set; }

        /// <summary>
        /// 挂号费
        /// </summary>
        public decimal RegFee { get; set; }

        /// <summary>
        /// 检查费
        /// </summary>
        public decimal CheckFee { get; set; }

        /// <summary>
        /// 诊察费
        /// </summary>
        public decimal DiagFee { get; set; }

        /// <summary>
        /// 附加费
        /// </summary>
        public decimal OtherFee { get; set; }

        /// <summary>
        /// 自费金额
        /// </summary>
        public decimal OwnCost { get; set; }

        /// <summary>
        /// 报销金额
        /// </summary>
        public decimal PubCost { get; set; }

        /// <summary>
        /// 自付金额
        /// </summary>
        public decimal PayCost { get; set; }

        /// <summary>
        /// 操作员代码
        /// </summary>
        public string OperCode { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperDate { get; set; }

        /// <summary>
        /// 医疗类别
        /// </summary>
        public string MedicalType { get; set; }

        /// <summary>
        /// 疾病代码
        /// </summary>
        public string IcdCode { get; set; }

        /// <summary>
        /// 打印发票数量
        /// </summary>
        public int PrintInvoicecnt { get; set; }

        /// <summary>
        /// 看诊医生代码
        /// </summary>
        public string SeeDoctorCode { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal EcoCost { get; set; }

        /// <summary>
        /// 是否急诊号
        /// </summary>
        public string IsEmergency { get; set; }

        /// <summary>
        /// 网络卡号
        /// </summary>
        public string NetCardno { get; set; }

        /// <summary>
        /// 转诊人
        /// </summary>
        public string ConvertOper { get; set; }

        /// <summary>
        /// 转诊时间
        /// </summary>
        public DateTime? ConvertDate { get; set; }

        /// <summary>
        /// 是否人工预约
        /// </summary>
        public string Isprebyreg { get; set; }

        /// <summary>
        /// 0挂号 1已诊  2退号
        /// </summary>
        public string SeeFlag { get; set; }

        /// <summary>
        /// 排班模板编码
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 体温
        /// </summary>
        public decimal Temperature { get; set; }

        /// <summary>
        /// 脉搏
        /// </summary>
        public decimal Pulse { get; set; }

        /// <summary>
        /// 呼吸率
        /// </summary>
        public decimal Breath { get; set; }

        /// <summary>
        /// 收缩压
        /// </summary>
        public decimal Systolic { get; set; }

        /// <summary>
        /// 舒张压
        /// </summary>
        public decimal Diastolic { get; set; }

        /// <summary>
        /// 是否日结 1已日结， 0未日结
        /// </summary>
        public string BalanceFlag { get; set; }

        /// <summary>
        /// 病历本费用
        /// </summary>
        public decimal EmrFee { get; set; }

        /// <summary>
        /// 病历本费用退费标志 1收费 0退费 2已打印病历本
        /// </summary>
        public string ReturnEmrfeeFlag { get; set; }

        /// <summary>
        /// 看诊时间
        /// </summary>
        public DateTime? SeeDate { get; set; }

        /// <summary>
        /// 看诊开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 看诊结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 看诊科室
        /// </summary>
        public string SeeDeptCode { get; set; }

        /// <summary>
        /// 代办人姓名
        /// </summary>
        public string AgentName { get; set; }

        /// <summary>
        /// 代办人性别
        /// </summary>
        public string AgentSex { get; set; }

        /// <summary>
        /// 代办人年龄
        /// </summary>
        public string AgentAge { get; set; }

        /// <summary>
        /// 代办人身份证
        /// </summary>
        public string AgentIdno { get; set; }

        /// <summary>
        /// 门诊转归类型
        /// </summary>
        public string LapseType { get; set; }

        /// <summary>
        /// 门诊转归时间
        /// </summary>
        public DateTime? LapseDate { get; set; }

        /// <summary>
        /// 排班ID
        /// </summary>
        public string SchemaId { get; set; }

        /// <summary>
        /// 号源池ID
        /// </summary>
        public string PoolId { get; set; }

        /// <summary>
        /// 医院编码
        /// </summary>
        public string HosUnitCode { get; set; }

        /// <summary>
        /// 预约ID
        /// </summary>
        public string RecId { get; set; }
    }
}
