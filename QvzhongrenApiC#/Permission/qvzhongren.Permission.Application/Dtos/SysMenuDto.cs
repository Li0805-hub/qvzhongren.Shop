namespace qvzhongren.Permission.Application.Dtos;

/// <summary>
/// 菜单响应DTO
/// </summary>
public class SysMenuResponseDto
{
    public string MenuId { get; set; }
    public string MenuName { get; set; }
    public string ParentId { get; set; }
    public string? Path { get; set; }
    public string? Component { get; set; }
    public string? Icon { get; set; }
    public string MenuType { get; set; }
    public string? Perms { get; set; }
    public int? SortNo { get; set; }
    public string Status { get; set; }
    public string? CreateCode { get; set; }
    public DateTime? CreateDate { get; set; }
    /// <summary>子菜单</summary>
    public List<SysMenuResponseDto>? Children { get; set; }
}

/// <summary>
/// 菜单创建DTO
/// </summary>
public class SysMenuCreateDto
{
    public string MenuId { get; set; }
    public string MenuName { get; set; }
    public string ParentId { get; set; } = "0";
    public string? Path { get; set; }
    public string? Component { get; set; }
    public string? Icon { get; set; }
    public string MenuType { get; set; } = "C";
    public string? Perms { get; set; }
    public int? SortNo { get; set; }
    public string Status { get; set; } = "1";
}
