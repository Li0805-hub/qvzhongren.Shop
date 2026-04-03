namespace qvzhongren.Contracts.Clients;

/// <summary>
/// 日志写入接口，每个服务注册自己的实现
/// </summary>
public interface ILogWriter
{
    Task LogAsync(string name, string type, string content, string? requestBody = null, string? responseBody = null);
    Task LogInformationAsync(string name, string content);
    Task LogWarningAsync(string name, string content);
    Task LogErrorAsync(string name, string content);
}
