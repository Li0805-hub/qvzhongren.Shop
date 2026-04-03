using qvzhongren.Web;

var builder = WebApplication.CreateBuilder(args);

// 配置 ABP 模块
builder.Host.UseAutofac(); // 使用 Autofac 作为 DI 容器
await builder.AddApplicationAsync<HisWebModule>(); // 添加 HisWebModule

// 添加基本服务
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// 初始化 ABP 应用
await app.InitializeApplicationAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseConfiguredEndpoints();

// 注册日志中间件
//app.UseMiddleware<LoggingMiddleware>();

await app.RunAsync();

