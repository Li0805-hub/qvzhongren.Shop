namespace qvzhongren.Contracts.Dtos;

public class SimpleUserDto
{
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string? RealName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? DeptCode { get; set; }
    public string? Status { get; set; }
}

public class SimpleRoleDto
{
    public string RoleId { get; set; } = "";
    public string? RoleCode { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}

public class SimpleMenuDto
{
    public string MenuId { get; set; } = "";
    public string? MenuName { get; set; }
    public string? Path { get; set; }
    public string? MenuType { get; set; }
    public string? Status { get; set; }
    public string? ParentId { get; set; }
}
