namespace qvzhongren.Model
{
    public class BaseEntity
    {

        /// <summary>
        /// 医院编号
        /// </summary>
        [SugarColumn(ColumnName = "HOS_UNIT_CODE")]
        public virtual string HosUnitCode { get; set; }

    }
}
