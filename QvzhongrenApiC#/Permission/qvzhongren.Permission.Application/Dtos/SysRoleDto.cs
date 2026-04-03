namespace qvzhongren.Permission.Application.Dtos;

/// <summary>
/// 角色响应DTO
/// </summary>
public class SysRoleResponseDto
{
    public string RoleId { get; set; }
    public string RoleCode { get; set; }
    public string RoleName { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public int? SortNo { get; set; }
    public string? CreateCode { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// 角色创建DTO
/// </summary>
public class SysRoleCreateDto
{
    public string RoleId { get; set; }
    public string RoleCode { get; set; }
    public string RoleName { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "1";
    public int? SortNo { get; set; }
}
