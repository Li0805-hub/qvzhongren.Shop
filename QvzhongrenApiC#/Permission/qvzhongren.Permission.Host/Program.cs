using qvzhongren.Permission.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();
await builder.AddApplicationAsync<PermissionHostModule>();

var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();
