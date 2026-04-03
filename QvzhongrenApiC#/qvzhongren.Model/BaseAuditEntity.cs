namespace qvzhongren.Model
{
    /// <summary>
    /// 基础实体类
    /// </summary>
    public class BaseAuditEntity: BaseUpdateInfoEntity
    {
        /// <summary>
        /// 创建人编码
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_CODE")]
        public virtual string CreateCode { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [SugarColumn(ColumnName = "CREATE_DATE")]
        public virtual DateTime CreateDate { get; set; }
    }
}
