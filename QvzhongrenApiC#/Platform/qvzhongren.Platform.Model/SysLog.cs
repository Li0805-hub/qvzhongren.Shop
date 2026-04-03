using qvzhongren.Model;
using SqlSugar;

namespace qvzhongren.Platform.Model;

/// <summary>
/// 系统日志表
/// </summary>
[SugarTable("SYS_LOG")]
public class SysLog : BaseEntity
{
    [SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
    public string Id { get; set; }

    [SugarColumn(ColumnName = "NAME")]
    public string Name { get; set; }

    [SugarColumn(ColumnName = "TYPE")]
    public string Type { get; set; }

    [SugarColumn(ColumnName = "CONTENT", ColumnDataType = "text")]
    public string Content { get; set; }

    [SugarColumn(ColumnName = "REQUEST_BODY", ColumnDataType = "text", IsNullable = true)]
    public string? RequestBody { get; set; }

    [SugarColumn(ColumnName = "RESPONSE_BODY", ColumnDataType = "text", IsNullable = true)]
    public string? ResponseBody { get; set; }

    [SugarColumn(ColumnName = "TIMESTAMP")]
    public DateTime Timestamp { get; set; }
}
