namespace qvzhongren.Platform.Application.Dtos;

public class ServiceConfigResponseDto
{
    public string ConfigId { get; set; } = "";
    public string ServiceName { get; set; } = "";
    public string ServiceUrl { get; set; } = "";
    public string? Description { get; set; }
    public string Status { get; set; } = "1";
    public int? SortNo { get; set; }
    public string? CreateCode { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? UpdateCode { get; set; }
    public DateTime? UpdateDate { get; set; }
}

public class ServiceConfigCreateDto
{
    public string? ConfigId { get; set; }
    public string ServiceName { get; set; } = "";
    public string ServiceUrl { get; set; } = "";
    public string? Description { get; set; }
    public string Status { get; set; } = "1";
    public int? SortNo { get; set; }
}
