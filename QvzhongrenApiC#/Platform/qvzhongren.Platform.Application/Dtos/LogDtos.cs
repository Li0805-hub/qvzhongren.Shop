namespace qvzhongren.Platform.Application.Dtos;

public class LogQueryDto
{
    public string? Type { get; set; }
    public string? Keyword { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class LogDeleteDto
{
    /// <summary>按时间范围删除：开始时间</summary>
    public DateTime? StartTime { get; set; }
    /// <summary>按时间范围删除：结束时间</summary>
    public DateTime? EndTime { get; set; }
    /// <summary>按类型删除</summary>
    public string? Type { get; set; }
    /// <summary>按关键字删除</summary>
    public string? Keyword { get; set; }
}
