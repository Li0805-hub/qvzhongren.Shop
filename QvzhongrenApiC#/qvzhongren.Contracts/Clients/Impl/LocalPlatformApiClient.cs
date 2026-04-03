using qvzhongren.Contracts.Dtos;
using SqlSugar;

namespace qvzhongren.Contracts.Clients.Impl;

/// <summary>
/// Platform 服务的本地直连实现（单体模式使用，直接查数据库）
/// </summary>
public class LocalPlatformApiClient : IPlatformApiClient
{
    private readonly ISqlSugarClient _db;

    public LocalPlatformApiClient(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<List<SimpleLogDto>> GetRecentLogsAsync(string? type, int count = 10)
    {
        var query = _db.Queryable<object>().AS("SYS_LOG").OrderBy("\"TIMESTAMP\" DESC");
        if (!string.IsNullOrEmpty(type))
            query = query.Where($"\"TYPE\" = '{type}'");
        return await query.Take(count)
            .Select<SimpleLogDto>("\"TYPE\" as Type, \"NAME\" as Name, \"CONTENT\" as Content, \"TIMESTAMP\" as Timestamp")
            .ToListAsync();
    }

    public async Task<int> GetLogCountAsync()
    {
        return await _db.Queryable<object>().AS("SYS_LOG").CountAsync();
    }
}
