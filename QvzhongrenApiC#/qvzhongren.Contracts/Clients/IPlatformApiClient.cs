using qvzhongren.Contracts.Dtos;

namespace qvzhongren.Contracts.Clients;

/// <summary>
/// Platform 服务的远程调用接口
/// </summary>
public interface IPlatformApiClient
{
    Task<List<SimpleLogDto>> GetRecentLogsAsync(string? type, int count = 10);
    Task<int> GetLogCountAsync();
}
