using SqlSugar;
using qvzhongren.Model;

namespace qvzhongren.Permission.Model;

/// <summary>
/// 用户账号表
/// </summary>
[SugarTable("SYS_USER")]
public class SysUser : BaseAuditEntity
{
    /// <summary>用户ID</summary>
    [SugarColumn(ColumnName = "USER_ID", IsPrimaryKey = true)]
    public string UserId { get; set; }

    /// <summary>用户名（登录账号）</summary>
    [SugarColumn(ColumnName = "USER_NAME")]
    public string UserName { get; set; }

    /// <summary>密码</summary>
    [SugarColumn(ColumnName = "PASSWORD")]
    public string Password { get; set; }

    /// <summary>真实姓名</summary>
    [SugarColumn(ColumnName = "REAL_NAME")]
    public string? RealName { get; set; }

    /// <summary>手机号</summary>
    [SugarColumn(ColumnName = "PHONE")]
    public string? Phone { get; set; }

    /// <summary>邮箱</summary>
    [SugarColumn(ColumnName = "EMAIL")]
    public string? Email { get; set; }

    /// <summary>部门编码</summary>
    [SugarColumn(ColumnName = "DEPT_CODE")]
    public string? DeptCode { get; set; }

    /// <summary>状态（1=启用 0=禁用）</summary>
    [SugarColumn(ColumnName = "STATUS")]
    public string Status { get; set; } = "1";

    /// <summary>备注</summary>
    [SugarColumn(ColumnName = "REMARK")]
    public string? Remark { get; set; }
}
