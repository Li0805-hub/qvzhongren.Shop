using qvzhongren.Contracts.Clients;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// 从数据库读取服务地址，覆盖 appsettings.json 中的 YARP Cluster 配置
var connStr = configuration["DbConnections:MasterDB:ConnectionString"] ?? "";
var serviceUrls = ServiceConfigLoader.GetAllServiceUrls(connStr);

// 服务名 → YARP Cluster 映射
var clusterMapping = new Dictionary<string, string>
{
    { "PermissionService", "ReverseProxy:Clusters:permission-cluster:Destinations:destination1:Address" },
    { "MessageService",    "ReverseProxy:Clusters:message-cluster:Destinations:destination1:Address" },
    { "AgentService",      "ReverseProxy:Clusters:agent-cluster:Destinations:destination1:Address" },
    { "PlatformService",   "ReverseProxy:Clusters:platform-cluster:Destinations:destination1:Address" },
};

// 用数据库读到的地址覆盖配置
var overrides = new Dictionary<string, string?>();
foreach (var (serviceName, configPath) in clusterMapping)
{
    if (serviceUrls.TryGetValue(serviceName, out var url))
    {
        overrides[configPath] = url;
    }
}
if (overrides.Count > 0)
{
    builder.Configuration.AddInMemoryCollection(overrides);
}

// 添加 YARP 反向代理
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayCors", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("GatewayCors");
app.UseRouting();

app.MapReverseProxy();

await app.RunAsync();
