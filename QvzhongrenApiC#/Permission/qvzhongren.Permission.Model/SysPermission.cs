using SqlSugar;
using qvzhongren.Model;

namespace qvzhongren.Permission.Model;

/// <summary>
/// 权限表
/// </summary>
[SugarTable("SYS_PERMISSION")]
public class SysPermission : BaseAuditEntity
{
    /// <summary>权限ID</summary>
    [SugarColumn(ColumnName = "PERMISSION_ID", IsPrimaryKey = true)]
    public string PermissionId { get; set; }

    /// <summary>权限编码</summary>
    [SugarColumn(ColumnName = "PERMISSION_CODE")]
    public string PermissionCode { get; set; }

    /// <summary>权限名称</summary>
    [SugarColumn(ColumnName = "PERMISSION_NAME")]
    public string PermissionName { get; set; }

    /// <summary>权限类型（menu=菜单 button=按钮 api=接口）</summary>
    [SugarColumn(ColumnName = "PERMISSION_TYPE")]
    public string? PermissionType { get; set; }

    /// <summary>描述</summary>
    [SugarColumn(ColumnName = "DESCRIPTION")]
    public string? Description { get; set; }

    /// <summary>状态（1=启用 0=禁用）</summary>
    [SugarColumn(ColumnName = "STATUS")]
    public string Status { get; set; } = "1";
}
