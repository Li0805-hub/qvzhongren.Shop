namespace qvzhongren.Permission.Application.Dtos;

public class DeptCreateDto
{
    public string DeptCode { get; set; } = "";
    public string DeptName { get; set; } = "";
    public string? ParentCode { get; set; }
    public int? SortNo { get; set; }
    public string? Status { get; set; }
    public string? Leader { get; set; }
    public string? Phone { get; set; }
}

public class DeptTreeDto
{
    public string DeptCode { get; set; } = "";
    public string DeptName { get; set; } = "";
    public string ParentCode { get; set; } = "0";
    public int? SortNo { get; set; }
    public string Status { get; set; } = "1";
    public string? Leader { get; set; }
    public string? Phone { get; set; }
    public DateTime? CreateDate { get; set; }
    public List<DeptTreeDto>? Children { get; set; }
}
