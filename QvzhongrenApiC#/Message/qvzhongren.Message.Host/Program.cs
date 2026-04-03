using qvzhongren.Message.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();
await builder.AddApplicationAsync<MessageHostModule>();

var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();
