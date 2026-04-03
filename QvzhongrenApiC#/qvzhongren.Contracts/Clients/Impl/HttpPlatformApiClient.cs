using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using qvzhongren.Contracts.Dtos;

namespace qvzhongren.Contracts.Clients.Impl;

/// <summary>
/// Platform 服务的 HTTP 远程调用实现（分布式模式使用）
/// </summary>
public class HttpPlatformApiClient : IPlatformApiClient
{
    private readonly HttpClient _http;

    public HttpPlatformApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<SimpleLogDto>> GetRecentLogsAsync(string? type, int count = 10)
    {
        var body = new { Type = type, PageIndex = 1, PageSize = count };
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var result = await _http.PostAsync("api/Platform/Log/GetPage", content);
        var response = await result.Content.ReadFromJsonAsync<ApiResult<PagedResult<SimpleLogDto>>>();
        return response?.Data?.Values ?? new List<SimpleLogDto>();
    }

    public async Task<int> GetLogCountAsync()
    {
        var body = new { PageIndex = 1, PageSize = 1 };
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var result = await _http.PostAsync("api/Platform/Log/GetPage", content);
        var response = await result.Content.ReadFromJsonAsync<ApiResult<PagedResult<SimpleLogDto>>>();
        return response?.Data?.TotalCount ?? 0;
    }
}

public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<T>? Values { get; set; }
}
