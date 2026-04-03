using SqlSugar;
using qvzhongren.Model;

namespace qvzhongren.Permission.Model;

/// <summary>
/// 用户菜单关联表（用户直接分配的菜单权限）
/// </summary>
[SugarTable("SYS_USER_MENU")]
public class SysUserMenu : BaseAuditEntity
{
    /// <summary>用户ID</summary>
    [SugarColumn(ColumnName = "USER_ID", IsPrimaryKey = true)]
    public string UserId { get; set; }

    /// <summary>菜单ID</summary>
    [SugarColumn(ColumnName = "MENU_ID", IsPrimaryKey = true)]
    public string MenuId { get; set; }
}
