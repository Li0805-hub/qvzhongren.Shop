using SqlSugar;
using qvzhongren.Model;

namespace qvzhongren.Permission.Model;

/// <summary>
/// 用户角色关联表
/// </summary>
[SugarTable("SYS_USER_ROLE")]
public class SysUserRole : BaseAuditEntity
{
    /// <summary>用户ID</summary>
    [SugarColumn(ColumnName = "USER_ID", IsPrimaryKey = true)]
    public string UserId { get; set; }

    /// <summary>角色ID</summary>
    [SugarColumn(ColumnName = "ROLE_ID", IsPrimaryKey = true)]
    public string RoleId { get; set; }
}
