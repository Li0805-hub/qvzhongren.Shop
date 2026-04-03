namespace qvzhongren.Permission.Application.Dtos;

/// <summary>
/// 权限响应DTO
/// </summary>
public class SysPermissionResponseDto
{
    public string PermissionId { get; set; }
    public string PermissionCode { get; set; }
    public string PermissionName { get; set; }
    public string? PermissionType { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public string? CreateCode { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// 权限创建DTO
/// </summary>
public class SysPermissionCreateDto
{
    public string PermissionId { get; set; }
    public string PermissionCode { get; set; }
    public string PermissionName { get; set; }
    public string? PermissionType { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "1";
}
