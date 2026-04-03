using SqlSugar;
using qvzhongren.Model;

namespace qvzhongren.Permission.Model;

/// <summary>
/// 角色菜单关联表
/// </summary>
[SugarTable("SYS_ROLE_MENU")]
public class SysRoleMenu : BaseAuditEntity
{
    /// <summary>角色ID</summary>
    [SugarColumn(ColumnName = "ROLE_ID", IsPrimaryKey = true)]
    public string RoleId { get; set; }

    /// <summary>菜单ID</summary>
    [SugarColumn(ColumnName = "MENU_ID", IsPrimaryKey = true)]
    public string MenuId { get; set; }
}
