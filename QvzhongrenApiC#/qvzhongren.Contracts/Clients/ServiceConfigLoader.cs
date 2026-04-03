using SqlSugar;

namespace qvzhongren.Contracts.Clients;

/// <summary>
/// 启动时从数据库加载服务地址配置
/// </summary>
public static class ServiceConfigLoader
{
    /// <summary>
    /// 从 SYS_SERVICE_CONFIG 表读取指定服务的地址
    /// </summary>
    /// <param name="connectionString">数据库连接串</param>
    /// <param name="serviceName">服务名称，如 PermissionService</param>
    /// <param name="fallbackUrl">数据库不可用时的备用地址</param>
    public static string GetServiceUrl(string connectionString, string serviceName, string fallbackUrl)
    {
        try
        {
            using var db = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = connectionString,
                DbType = DbType.PostgreSQL,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings { PgSqlIsAutoToLower = false }
            });

            var url = db.Queryable<object>()
                .AS("SYS_SERVICE_CONFIG")
                .Where("\"SERVICE_NAME\" = @name AND \"STATUS\" = '1'", new { name = serviceName })
                .Select<string>("\"SERVICE_URL\"")
                .First();

            if (!string.IsNullOrEmpty(url))
            {
                Console.WriteLine($"[ServiceConfig] {serviceName} = {url} (from database)");
                return url;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ServiceConfig] Failed to load {serviceName} from database: {ex.Message}");
        }

        Console.WriteLine($"[ServiceConfig] {serviceName} = {fallbackUrl} (fallback)");
        return fallbackUrl;
    }

    /// <summary>
    /// 从数据库读取所有启用的服务配置
    /// </summary>
    public static Dictionary<string, string> GetAllServiceUrls(string connectionString)
    {
        var result = new Dictionary<string, string>();
        try
        {
            using var db = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = connectionString,
                DbType = DbType.PostgreSQL,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings { PgSqlIsAutoToLower = false }
            });

            var configs = db.Queryable<object>()
                .AS("SYS_SERVICE_CONFIG")
                .Where("\"STATUS\" = '1'")
                .Select<ServiceUrlItem>("\"SERVICE_NAME\" as ServiceName, \"SERVICE_URL\" as ServiceUrl")
                .ToList();

            foreach (var c in configs)
            {
                result[c.ServiceName] = c.ServiceUrl;
                Console.WriteLine($"[ServiceConfig] {c.ServiceName} = {c.ServiceUrl}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ServiceConfig] Failed to load configs from database: {ex.Message}");
        }
        return result;
    }

    private class ServiceUrlItem
    {
        public string ServiceName { get; set; } = "";
        public string ServiceUrl { get; set; } = "";
    }
}
