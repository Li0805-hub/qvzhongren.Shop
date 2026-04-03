using SqlSugar;
using qvzhongren.Model;

namespace qvzhongren.Permission.Model;

/// <summary>
/// 角色表
/// </summary>
[SugarTable("SYS_ROLE")]
public class SysRole : BaseAuditEntity
{
    /// <summary>角色ID</summary>
    [SugarColumn(ColumnName = "ROLE_ID", IsPrimaryKey = true)]
    public string RoleId { get; set; }

    /// <summary>角色编码</summary>
    [SugarColumn(ColumnName = "ROLE_CODE")]
    public string RoleCode { get; set; }

    /// <summary>角色名称</summary>
    [SugarColumn(ColumnName = "ROLE_NAME")]
    public string RoleName { get; set; }

    /// <summary>描述</summary>
    [SugarColumn(ColumnName = "DESCRIPTION")]
    public string? Description { get; set; }

    /// <summary>状态（1=启用 0=禁用）</summary>
    [SugarColumn(ColumnName = "STATUS")]
    public string Status { get; set; } = "1";

    /// <summary>排序</summary>
    [SugarColumn(ColumnName = "SORT_NO")]
    public int? SortNo { get; set; }
}
