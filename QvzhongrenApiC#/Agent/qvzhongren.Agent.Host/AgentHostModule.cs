using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SqlSugar;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using qvzhongren.Agent.Application;
using qvzhongren.Platform.Application.Services;
using qvzhongren.Application.Common;
using qvzhongren.Application.Interceptors;
using qvzhongren.Application.Logging;
using qvzhongren.Contracts.Clients;
using qvzhongren.Contracts.Clients.Impl;
using qvzhongren.Repository.SqlSugar;
using qvzhongren.Shared.Common;
using qvzhongren.Shared.Helper;

namespace qvzhongren.Agent.Host;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AgentApplicationModule)
)]
public class AgentHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var configuration = context.Services.GetConfiguration();

        // 注册 AppSettings
        services.AddSingleton(new AppSettings(configuration));

        // 注册 LoggingInterceptor
        services.AddTransient<LoggingInterceptor>();

        // 注册 ILogWriter
        services.AddScoped<ILogWriter, DefaultLogWriter>();

        // 注册日志
        services.AddScoped<Logger>();

        // 注册 HttpClientFactory（用于智能助手调用DeepSeek API）
        services.AddHttpClient();

        // 注册跨服务接口（微服务模式：从数据库读取地址，appsettings 作为 fallback）
        var connStr = configuration["DbConnections:MasterDB:ConnectionString"] ?? "";
        var fallbackPermission = configuration["ServiceUrls:PermissionService"] ?? "http://localhost:5001";
        var fallbackPlatform = configuration["ServiceUrls:PlatformService"] ?? "http://localhost:5004";
        var permissionBaseUrl = ServiceConfigLoader.GetServiceUrl(connStr, "PermissionService", fallbackPermission);
        var platformBaseUrl = ServiceConfigLoader.GetServiceUrl(connStr, "PlatformService", fallbackPlatform);

        services.AddHttpClient<IPermissionApiClient, HttpPermissionApiClient>(client =>
        {
            client.BaseAddress = new Uri(permissionBaseUrl);
        });
        services.AddHttpClient<IPlatformApiClient, HttpPlatformApiClient>(client =>
        {
            client.BaseAddress = new Uri(platformBaseUrl);
        });

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.SetIsOriginAllowed(_ => true)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });

        services.AddAutoMapper(typeof(AgentHostModule));

        // 添加控制器服务
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
            })
            .AddControllersAsServices();

        // 配置动态API控制器
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(AgentHostModule).Assembly);
        });

        // 注册 SqlSugar 配置
        services.Configure<ConnectionConfig>(options =>
        {
            options.ConnectionString = AppSettings.GetValue("DbConnections:MasterDB:ConnectionString");
            options.DbType = (DbType)AppSettings.GetValue("DbConnections:MasterDB:DatabaseType").ConvertToInt();
            options.IsAutoCloseConnection = true;
            options.InitKeyType = InitKeyType.Attribute;
            options.MoreSettings = new ConnMoreSettings
            {
                IsAutoRemoveDataCache = true,
                IsWithNoLockQuery = false,
                PgSqlIsAutoToLower = false
            };
        });
        services.AddScoped<ISqlSugarClient>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<ConnectionConfig>>().Value;
            return new SqlSugarClient(config);
        });
        services.AddScoped<SqlSugarDbContext>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        // 动态获取所有包含Application的程序集的映射配置
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName != null && a.FullName.StartsWith("qvzhongren") && a.FullName.Contains("Application"))
            .ToArray();
        services.AddAutoMapper(assemblies);

        // FluentValidation
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblies(assemblies);

        // 配置 Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Agent API",
                Version = "v1",
                Description = "Agent 微服务接口文档"
            });

            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.AllDirectories);
            foreach (var xmlFile in xmlFiles)
            {
                options.IncludeXmlComments(xmlFile);
            }

            options.DocInclusionPredicate((docName, description) => !description.RelativePath.Contains("abp"));

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });

            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });

        // 配置 JWT 认证
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var secretKey = AppSettings.GetValue("Jwt:SecretKey");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AppSettings.GetValue("Jwt:Issuer"),
                ValidAudience = AppSettings.GetValue("Jwt:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Headers["Authorization"].ToString();
                    if (token.StartsWith("Bearer Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Token = token.Substring("Bearer Bearer ".Length).Trim();
                    }
                    else if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Token = token.Substring("Bearer ".Length).Trim();
                    }
                    return Task.CompletedTask;
                }
            };
        });

        // 注册当前用户服务
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<CurrentUserService>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseCors("AllowAll");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agent API V1");
            c.EnableFilter();
            c.DocExpansion(DocExpansion.None);
            c.DefaultModelsExpandDepth(-1);
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
