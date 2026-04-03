using SqlSugar;
using qvzhongren.Model;

namespace qvzhongren.Permission.Model;

[SugarTable("SYS_DEPT")]
public class SysDept : BaseAuditEntity
{
    [SugarColumn(ColumnName = "DEPT_CODE", IsPrimaryKey = true)]
    public string DeptCode { get; set; }

    [SugarColumn(ColumnName = "DEPT_NAME")]
    public string DeptName { get; set; }

    [SugarColumn(ColumnName = "PARENT_CODE")]
    public string ParentCode { get; set; } = "0";

    [SugarColumn(ColumnName = "SORT_NO")]
    public int? SortNo { get; set; }

    [SugarColumn(ColumnName = "STATUS")]
    public string Status { get; set; } = "1";

    [SugarColumn(ColumnName = "LEADER")]
    public string? Leader { get; set; }

    [SugarColumn(ColumnName = "PHONE")]
    public string? Phone { get; set; }
}
