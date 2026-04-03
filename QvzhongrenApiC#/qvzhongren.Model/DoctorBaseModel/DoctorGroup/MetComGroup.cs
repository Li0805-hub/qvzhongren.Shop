namespace qvzhongren.Model.DoctorBaseModel
{
    /// <summary>
    /// 医嘱组套主表
    ///</summary>
    [SugarTable("MET_COM_GROUP")]
    public class MetComGroup
    {


        /// <summary>
        /// 备  注:组套ID
        /// 默认值: seq_order_groupid
        ///</summary>
        [SugarColumn(ColumnName = "GROUP_ID", IsPrimaryKey = true)]
        public string GroupId { get; set; } = null!;

        /// <summary>
        /// 备  注:组套名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "GROUP_NAME")]
        public string GroupName { get; set; } = null!;

        /// <summary>
        /// 备  注:拼音码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "SPELL_CODE")]
        public string? SpellCode { get; set; }

        /// <summary>
        /// 备  注:五笔码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "WB_CODE")]
        public string? WbCode { get; set; }

        /// <summary>
        /// 备  注:1门诊/2住院
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "GROUP_TYPE")]
        public string? GroupType { get; set; }

        /// <summary>
        /// 备  注:组套类型,1.医师组套；2.科室组套
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "GROUP_KIND")]
        public string GroupKind { get; set; } = null!;

        /// <summary>
        /// 备  注:1西药 2中药 3检验 4检查 5综合
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "GROUP_CLASS")]
        public string? GroupClass { get; set; }

        /// <summary>
        /// 备  注:0 普通 1处方
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PRESC_FLAG")]
        public string? PrescFlag { get; set; }

        /// <summary>
        /// 备  注:授权科室
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DEPT_CODE")]
        public string? DeptCode { get; set; }

        /// <summary>
        /// 备  注:授权医生
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "DOCTOR_CODE")]
        public string? DoctorCode { get; set; }

        /// <summary>
        /// 备  注:备注
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "REMARK")]
        public string? Remark { get; set; }

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
        /// 备  注:院区编码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "HOS_UNIT_CODE")]
        public string? HosUnitCode { get; set; }

        /// <summary>
        /// 备  注:处方属性  毒麻 精神 普通等
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "PRESC_ATTR")]
        public string? PrescAttr { get; set; }


    }

}