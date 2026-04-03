namespace qvzhongren.Model
{
    public class BaseUpdateInfoEntity : BaseEntity
    {
        /// <summary>
        /// 修改人编码
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_CODE")]
        public virtual string UpdateCode { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        [SugarColumn(ColumnName = "UPDATE_DATE")]
        public virtual DateTime UpdateDate { get; set; }
    }
}
