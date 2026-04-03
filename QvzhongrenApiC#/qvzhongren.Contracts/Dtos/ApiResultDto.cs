namespace qvzhongren.Contracts.Dtos;

/// <summary>
/// 跨服务通信的通用返回包装
/// </summary>
public class ApiResult<T>
{
    public bool IsSuccess { get; set; }
    public int Code { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}
