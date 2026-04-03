namespace qvzhongren.Permission.Application.Dtos;

/// <summary>
/// 用户响应DTO
/// </summary>
public class SysUserResponseDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string? RealName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? DeptCode { get; set; }
    public string Status { get; set; }
    public string? Remark { get; set; }
    public string? CreateCode { get; set; }
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// 用户创建DTO
/// </summary>
public class SysUserCreateDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? RealName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? DeptCode { get; set; }
    public string Status { get; set; } = "1";
    public string? Remark { get; set; }
}
