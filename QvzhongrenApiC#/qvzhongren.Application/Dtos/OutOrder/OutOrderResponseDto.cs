
namespace qvzhongren.Application.Dtos.OutOrder
{
    /// <summary>
    /// 门诊医嘱响应Dto
    /// </summary>
    public class OutOrderResponseDto
    {

        /// <summary>
        /// 备  注:就诊流水号
        /// 默认值:
        ///</summary>
        public string ClinicCode { get; set; } = null!;

        /// <summary>
        /// 备  注:就诊卡号
        /// 默认值:
        ///</summary>
        public string PatientNo { get; set; } = null!;

        /// <summary>
        /// 备  注:挂号日期
        /// 默认值:
        ///</summary>
        public DateTime? RegDate { get; set; }

        /// <summary>
        /// 备  注:挂号科室
        /// 默认值:
        ///</summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// 备  注:处方号 (序列号)
        /// 默认值:  seq_outp_prescno
        ///</summary>
        public string PrescNo { get; set; } = null!;


        /// <summary>
        /// 处方组号
        /// </summary>
        public string PrescCombNo { get; set; }
        /// <summary>
        /// 备  注:医嘱序号
        /// 默认值:
        ///</summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 备  注:医嘱子序号
        /// 默认值:
        ///</summary>
        public int OrderSubNo { get; set; }

        /// <summary>
        /// 备  注:医嘱唯一序列号
        /// 默认值:  seq_fin_moorderno
        ///</summary>
        public string MoOrder { get; set; } 

        /// <summary>
        /// 备  注:医嘱状态 1开立 2收费 3发药 0作废
        /// 默认值:
        ///</summary>
        public string OrderState { get; set; }

        /// <summary>
        /// 备  注:开立医生
        /// 默认值:
        ///</summary>
        public string DoctorCode { get; set; }

        /// <summary>
        /// 备  注:开立科室
        /// 默认值:
        ///</summary>
        public string OrderDept { get; set; }

        /// <summary>
        /// 备  注:开立时间
        /// 默认值:
        ///</summary>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 备  注:组合标志
        /// 默认值:
        ///</summary>
        public string CombFlag { get; set; }

        /// <summary>
        /// 备  注:类别编码
        /// 默认值:
        ///</summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// 备  注:项目编码
        /// 默认值:
        ///</summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 备  注:项目名称
        /// 默认值:
        ///</summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 备  注:规格
        /// 默认值:
        ///</summary>
        public string Specs { get; set; }

        /// <summary>
        /// 备  注:开立剂量
        /// 默认值:
        ///</summary>
        public decimal? OpenOnce { get; set; }

        /// <summary>
        /// 备  注:开立单位
        /// 默认值:
        ///</summary>
        public string OpenUnit { get; set; }

        /// <summary>
        /// 备  注:每次剂量
        /// 默认值:
        ///</summary>
        public decimal? OnceDose { get; set; }

        /// <summary>
        /// 备  注:每次量单位
        /// 默认值:
        ///</summary>
        public string DoseUnit { get; set; }

        /// <summary>
        /// 备  注:基本剂量
        /// 默认值:
        ///</summary>
        public decimal? BaseDose { get; set; }

        /// <summary>
        /// 备  注:频次
        /// 默认值:
        ///</summary>
        public string FrequencyCode { get; set; }

        /// <summary>
        /// 频次名称
        /// </summary>
        public string FrequencyName { get; set; }

        /// <summary>
        /// 备  注:执行次数
        /// 默认值:
        ///</summary>
        public int? FrequencyCounts { get; set; }

        /// <summary>
        /// 备  注:用法
        /// 默认值:
        ///</summary>
        public string UsageCode { get; set; }

        /// <summary>
        /// 用法名称
        /// </summary>
        public string UsageName { get; set; }

        /// <summary>
        /// 备  注:滴速
        /// 默认值:
        ///</summary>
        public string Drippingspeed { get; set; }

        /// <summary>
        /// 备  注:使用天数
        /// 默认值:
        ///</summary>
        public int? UseDays { get; set; }

        /// <summary>
        /// 备  注:总量
        /// 默认值:
        ///</summary>
        public decimal? TotQty { get; set; }

        /// <summary>
        /// 备  注:总量单位
        /// 默认值:
        ///</summary>
        public string TotUnit { get; set; }

        /// <summary>
        /// 备  注:总价
        /// 默认值:
        ///</summary>
        public decimal? TotCost { get; set; }

        /// <summary>
        /// 备  注:医生说明
        /// 默认值:
        ///</summary>
        public string DoctorNote { get; set; }

        /// <summary>
        /// 备  注:单价
        /// 默认值:
        ///</summary>
        public decimal ItemPrice { get; set; }

        /// <summary>
        /// 备  注:执行科室/取药药房
        /// 默认值:
        ///</summary>
        public string ExecDept { get; set; }

        /// <summary>
        /// 执行科室名称
        /// </summary>
        public string ExecDeptName { get; set; }


        /// <summary>
        /// 备  注:药品性质（毒麻 精神等）
        /// 默认值:
        ///</summary>
        public string DrugQuality { get; set; }

        /// <summary>
        /// 备  注:皮试 0不用皮试，1皮试，未做 2阳性 3阴性
        /// 默认值:
        ///</summary>
        public string SkinTest { get; set; }

        /// <summary>
        /// 备  注:抗菌药物标志
        /// 默认值:
        ///</summary>
        public string AntibioticsFlag { get; set; }

        /// <summary>
        /// 备  注:抗菌药物目的
        /// 默认值:
        ///</summary>
        public string AntibioticsIntent { get; set; }

        /// <summary>
        /// 备  注:是否可拆分
        /// 默认值:
        ///</summary>
        public string SplitType { get; set; }

        /// <summary>
        /// 备  注:是否手动计算药品总量
        /// 默认值:
        ///</summary>
        public string Iscalu { get; set; }

        /// <summary>
        /// 备  注:用药方式
        /// 默认值:
        ///</summary>
        public string AdministrationCode { get; set; }

        /// <summary>
        /// 备  注:煎药方法
        /// 默认值:
        ///</summary>
        public string DecoctionCode { get; set; }

        /// <summary>
        /// 备  注:加工方法
        /// 默认值:
        ///</summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 备  注:下药方法
        /// 默认值:
        ///</summary>
        public string MedicineCode { get; set; }

        /// <summary>
        /// 备  注:剂数  每次多少剂
        /// 默认值:
        ///</summary>
        public int? DosageCount { get; set; }

        /// <summary>
        /// 备  注:中药剂型
        /// 默认值:
        ///</summary>
        public string DosageForm { get; set; }

        /// <summary>
        /// 备  注:最小单位
        /// 默认值:
        ///</summary>
        public string MinUnit { get; set; }

        /// <summary>
        /// 备  注:包装数量
        /// 默认值:
        ///</summary>
        public int PackQty { get; set; }

        /// <summary>
        /// 备  注:包装单位
        /// 默认值:
        ///</summary>
        public string PackUnit { get; set; }

        /// <summary>
        /// 备  注:取消医生
        /// 默认值:
        ///</summary>
        public string CancelDoctor { get; set; }

        /// <summary>
        /// 备  注:取消时间
        /// 默认值:
        ///</summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 备  注:皮试护士
        /// 默认值:
        ///</summary>
        public string SkinNurse { get; set; }

        /// <summary>
        /// 备  注:皮试时间
        /// 默认值:
        ///</summary>
        public DateTime? SkinDate { get; set; }

        /// <summary>
        /// 备  注:执行护士
        /// 默认值:
        ///</summary>
        public string ExecuteNurse { get; set; }

        /// <summary>
        /// 备  注:执行时间
        /// 默认值:
        ///</summary>
        public DateTime? ExecuteDate { get; set; }

        /// <summary>
        /// 备  注:打印次数
        /// 默认值:
        ///</summary>
        public int PrintCount { get; set; }

        /// <summary>
        /// 备  注:计价属性
        /// 默认值:
        ///</summary>
        public string DrugChargeAttr { get; set; }

        /// <summary>
        /// 备  注:生产厂家
        /// 默认值:
        ///</summary>
        public string ProducerCode { get; set; }

        /// <summary>
        /// 备  注:最小费用代码
        /// 默认值:
        ///</summary>
        public string FeeCode { get; set; }

        /// <summary>
        /// 备  注:医保等级
        /// 默认值:
        ///</summary>
        public string ItemYbGrade { get; set; }


        /// <summary>
        /// 备  注:药品标志 1是 0否
        /// 默认值:
        ///</summary>
        public string DrugFlag { get; set; }

        /// <summary>
        /// 备  注:加急标志
        /// 默认值:
        ///</summary>
        public string UrgentFlag { get; set; }

        /// <summary>
        /// 备  注:申请单号
        /// 默认值:
        ///</summary>
        public string ApplyNo { get; set; }

        /// <summary>
        /// 备  注:检查检验申请说明
        /// 默认值:
        ///</summary>
        public string ItemMemo { get; set; }

        /// <summary>
        /// 备  注:检查方法编码
        /// 默认值:
        ///</summary>
        public string ExamMethod { get; set; }

        /// <summary>
        /// 备  注:是否创建申请单
        /// 默认值:
        ///</summary>
        public string CreateApplyForm { get; set; }

        /// <summary>
        /// 备  注:是否需要终端确认
        /// 默认值:
        ///</summary>
        public string IsTerminalConfirm { get; set; }

        /// <summary>
        /// 备  注:
        /// 默认值:
        ///</summary>
        public string DiagCode { get; set; }


        public string MedicineName { get; set; }

        /// <summary>
        /// 操作状态 1:新增 2:修改 3:删除
        /// </summary>
        public string OperationState { get; set; }

        /// <summary>
        /// 患者检查申请信息
        /// </summary>
        public PacsApplyInfoResponseDto? PacsApplicationinfo { get; set; }

        /// <summary>
        /// 检验申请信息
        /// </summary>
        public LisApplyInfoResponseDto?  LisApplyInfo { get; set; }

        /// <summary>
        /// 病理申请信息
        /// </summary>
        public PathologyApplyinfoResponseDto? PathologyApplyinfo { get; set; }


		/// <summary>
		/// 适应症说明
		/// </summary>
		public string Remark { get; set; }

	}
}
