using qvzhongren.Contracts.Clients;
using qvzhongren.Platform.Model;
using qvzhongren.Shared.Helper;
using SqlSugar;

namespace qvzhongren.Platform.Application.Services;

/// <summary>
/// 默认日志写入实现，直接写入 PostgreSQL 的 SYS_LOG 表
/// </summary>
public class DefaultLogWriter : ILogWriter
{
    private static ConnectionConfig GetConfig()
    {
        return new ConnectionConfig
        {
            ConnectionString = AppSettings.GetValue("DbConnections:MasterDB:ConnectionString"),
            DbType = (DbType)AppSettings.GetValue("DbConnections:MasterDB:DatabaseType").ConvertToInt(),
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute,
            MoreSettings = new ConnMoreSettings
            {
                PgSqlIsAutoToLower = false
            }
        };
    }

    public async Task LogInformationAsync(string name, string content)
    {
        await LogAsync(name, "Information", content);
    }

    public async Task LogWarningAsync(string name, string content)
    {
        await LogAsync(name, "Warning", content);
    }

    public async Task LogErrorAsync(string name, string content)
    {
        await LogAsync(name, "Error", content);
    }

    public async Task LogAsync(string name, string type, string content, string? requestBody = null, string? responseBody = null)
    {
        try
        {
            using var db = new SqlSugarClient(GetConfig());
            var log = new SysLog
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name,
                Type = type,
                Content = content,
                RequestBody = Truncate(requestBody, 8000),
                ResponseBody = Truncate(responseBody, 8000),
                Timestamp = DateTime.Now
            };
            await db.Insertable(log).ExecuteCommandAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"日志写入失败: {ex.Message}");
        }
    }

    private static string? Truncate(string? s, int max)
    {
        if (s == null) return null;
        return s.Length <= max ? s : s[..max] + "...(truncated)";
    }
}
