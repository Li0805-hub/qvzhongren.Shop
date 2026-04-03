using qvzhongren.Platform.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();
await builder.AddApplicationAsync<PlatformHostModule>();

var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();
