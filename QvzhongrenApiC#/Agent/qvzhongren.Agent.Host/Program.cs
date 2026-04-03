using qvzhongren.Agent.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();
await builder.AddApplicationAsync<AgentHostModule>();

var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();
