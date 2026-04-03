namespace qvzhongren.Model.DoctorBaseModel
{
    /// <summary>
    /// 医嘱组套明细表
    ///</summary>
    [SugarTable("MET_COM_GROUPDETAIL")]
    public class MetComGroupdetail
    {


        /// <summary>
        /// 备  注:组套流水号
        /// 默认值: seq_order_groupid
        ///</summary>
        [SugarColumn(ColumnName = "GROUP_ID", IsPrimaryKey = true)]
        public string GroupId { get; set; } = null!;

        /// <summary>
        /// 备  注:医嘱编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ITEM_CODE")]
        public string? ItemCode { get; set; }

        /// <summary>
        /// 备  注:医嘱名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ITEM_NAME")]
        public string? ItemName { get; set; }

        /// <summary>
        /// 备  注:频次编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "FREQUENCY_CODE")]
        public string? FrequencyCode { get; set; }

        /// <summary>
        /// 备  注:用法编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "USAGE_CODE")]
        public string? UsageCode { get; set; }

        /// <summary>
        /// 备  注:每次量
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ONCE_DOSE")]
        public double? OnceDose { get; set; }

        /// <summary>
        /// 备  注:系统编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CLASS_CODE")]
        public string? ClassCode { get; set; }

        /// <summary>
        /// 备  注:医嘱序号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ORDER_NO")]
        public short? OrderNo { get; set; }

        /// <summary>
        /// 备  注:医嘱子序号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ORDER_SUB_NO")]
        public short? OrderSubNo { get; set; }

        /// <summary>
        /// 备  注:长期医嘱标志 0 临时 1长期
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DECMPS_STATE")]
        public string? DecmpsState { get; set; }

        /// <summary>
        /// 备  注:频次执行次数
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "FREQUENCY_COUNTS")]
        public short? FrequencyCounts { get; set; }

        /// <summary>
        /// 备  注:药品计价属性 1正常摆药 ，2自带药，3不摆药，4出院带药
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DRUG_CHARGE_ATTR")]
        public string? DrugChargeAttr { get; set; }

        /// <summary>
        /// 备  注:医生备注
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DOCTOR_NOTE")]
        public string? DoctorNote { get; set; }

        /// <summary>
        /// 备  注:组合标志
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "COMB_FLAG")]
        public string? CombFlag { get; set; }

        /// <summary>
        /// 备  注:总量
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "TOT_QTY")]
        public double? TotQty { get; set; }

        /// <summary>
        /// 备  注:项目单位
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ITEM_UNIT")]
        public string? ItemUnit { get; set; }

        /// <summary>
        /// 备  注:天数（付数）
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "USE_DAYS")]
        public short? UseDays { get; set; }

        /// <summary>
        /// 备  注:执行科室
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "EXEC_DEPT")]
        public string? ExecDept { get; set; }

        /// <summary>
        /// 备  注:取药药房
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PHARMACY_CODE")]
        public string? PharmacyCode { get; set; }

        /// <summary>
        /// 备  注:包装单位
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PACK_UNIT")]
        public string? PackUnit { get; set; }

        /// <summary>
        /// 备  注:规格
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "SPECS")]
        public string? Specs { get; set; }

        /// <summary>
        /// 备  注:包装数量
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PACK_QTY")]
        public short? PackQty { get; set; }

        /// <summary>
        /// 备  注:操作人
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "OPER_CODE")]
        public string? OperCode { get; set; }

        /// <summary>
        /// 备  注:操作时间
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "OPER_DATE")]
        public DateTime? OperDate { get; set; }

        /// <summary>
        /// 备  注:用药方式编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ADMINISTRATION_CODE")]
        public string? AdministrationCode { get; set; }

        /// <summary>
        /// 备  注:煎药方法编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DECOCTION_CODE")]
        public string? DecoctionCode { get; set; }

        /// <summary>
        /// 备  注:下药方式编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "MEDICINE_CODE")]
        public string? MedicineCode { get; set; }

        /// <summary>
        /// 备  注:用药说明  中药
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "MEDICINE_NOTE")]
        public string? MedicineNote { get; set; }

        /// <summary>
        /// 备  注:剂数 每次多少剂
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DOSAGE_COUNT")]
        public short? DosageCount { get; set; }

        /// <summary>
        /// 备  注:加工方法编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PROCESS_CODE")]
        public string? ProcessCode { get; set; }

        /// <summary>
        /// 备  注:中药剂型
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DOSAGE_FORM")]
        public string? DosageForm { get; set; }

        /// <summary>
        /// 备  注:院区编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "HOS_UNIT_CODE")]
        public string? HosUnitCode { get; set; }

        /// <summary>
        /// 备  注:计价属性    1正常计价，2护士手工计价，3不计价
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "CHARGE_ATTR")]
        public string? ChargeAttr { get; set; }

        /// <summary>
        /// 备  注:开立单位
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "OPEN_UNIT")]
        public string? OpenUnit { get; set; }

        /// <summary>
        /// 备  注:每次量（开立）
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "OPEN_ONCE")]
        public double? OpenOnce { get; set; }

        /// <summary>
        /// 备  注:单价
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ITEM_PRICE")]
        public double? ItemPrice { get; set; }

        /// <summary>
        /// 备  注:基本剂量
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "BASE_DOSE")]
        public double? BaseDose { get; set; }

        /// <summary>
        /// 备  注:剂量单位
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DOSE_UNIT")]
        public string? DoseUnit { get; set; }

        /// <summary>
        /// 备  注:最小单位
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "MIN_UNIT")]
        public string? MinUnit { get; set; }

        /// <summary>
        /// 备  注:药品标志
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DRUG_FLAG")]
        public string? DrugFlag { get; set; }

        /// <summary>
        /// 备  注:总金额
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "TOT_COST")]
        public double? TotCost { get; set; }

        /// <summary>
        /// 备  注:总量单位
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "TOT_UNIT")]
        public string? TotUnit { get; set; }

        /// <summary>
        /// 备  注:序列号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "SEQ_ORDER", IsPrimaryKey = true)]
        public string SeqOrder { get; set; } = null!;

        /// <summary>
        /// 备  注:药品性质
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DRUG_QUALITY")]
        public string? DrugQuality { get; set; }

        /// <summary>
        /// 备  注:是否手动计算药品总量
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ISCALU_FLAG")]
        public string? IscaluFlag { get; set; }


    }

}