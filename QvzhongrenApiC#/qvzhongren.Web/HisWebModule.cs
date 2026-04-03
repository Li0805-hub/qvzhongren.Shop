using Autofac;
using CSRedis;
using FluentValidation;
using FluentValidation.AspNetCore;
using qvzhongren.Application.Common;
using qvzhongren.Application.Dtos;
using qvzhongren.Application.Interceptors;
using qvzhongren.Permission.Application;
using qvzhongren.Message.Application;
using qvzhongren.Agent.Application;
using qvzhongren.Platform.Application;
using qvzhongren.Platform.Application.Services;
using qvzhongren.Shop.Application;
using qvzhongren.Application.Logging;
using qvzhongren.Contracts.Clients;
using qvzhongren.Contracts.Clients.Impl;
using qvzhongren.Repository.SqlSugar;
using qvzhongren.Shared.Common;
using qvzhongren.Shared.Helper;
using qvzhongren.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SqlSugar;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System.Text.Json.Serialization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
namespace qvzhongren.Web;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpEmailingModule),
    typeof(PermissionApplicationModule),
    typeof(MessageApplicationModule),
    typeof(AgentApplicationModule),
    typeof(PlatformApplicationModule),
    typeof(ShopApplicationModule)
    )]
public class HisWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var configuration = context.Services.GetConfiguration();

        // 注册 AppSettings
        services.AddSingleton(new AppSettings(configuration));

        // 注册 LoggingInterceptor
        services.AddTransient<LoggingInterceptor>();


    
        // 注册跨服务接口（单体模式：本地直连）
        services.AddScoped<IPermissionApiClient, LocalPermissionApiClient>();
        services.AddScoped<IPlatformApiClient, LocalPlatformApiClient>();
        services.AddScoped<ILogWriter, DefaultLogWriter>();

        //注册日志
        services.AddScoped<Logger>();

        // 注册 HttpClientFactory（用于智能助手调用DeepSeek API）
        services.AddHttpClient();

        // 注册 SignalR（实时消息）
        services.AddSignalR();

        // CORS（SignalR WebSocket 需要）
        services.AddCors(options =>
        {
            options.AddPolicy("SignalRCors", builder =>
            {
                builder.SetIsOriginAllowed(_ => true)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });

        services.AddAutoMapper(typeof(HisWebModule));

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
            options.ConventionalControllers.Create(typeof(HisWebModule).Assembly);
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


        // 注册 SqlSugar 客户端
        services.AddScoped<ISqlSugarClient>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<ConnectionConfig>>().Value;
            return new SqlSugarClient(config, db =>
            {
                db.Aop.OnLogExecuted =  (sql, p) =>
                {
                    string logSql = $"【SQL语句】{sql}" + "\r\n" +$"{GetParas(p)}" +
                    $"【耗时】{db.Ado.SqlExecutionTime.TotalMilliseconds}ms";
                    Console.WriteLine(logSql);
                };
            });
        });

        //注册 Redis 服务（本地无 Redis 时注释掉此行）
        //RedisHelper.Initialization(new CSRedisClient(configuration["Redis:MasterDB:ConnectionString"]));

        //services.AddScoped<RedisService>();
        // 注册数据库上下文和仓储
        services.AddScoped<SqlSugarDbContext>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        // 动态获取所有包含Application的程序集的映射配置
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName.StartsWith("qvzhongren") && a.FullName.Contains("Application"))
            .ToArray();
        services.AddAutoMapper(assemblies);

        //FluentValidation
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblies(assemblies);

        //拦截器
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                string msg = string.Join("；", errors.Select(e => $"{e.Key}：{string.Join("，", e.Value)}"));
                var errorResponse = ResultDto<object>.BadRequest(msg);
                return new JsonResult(errorResponse);
            };
        });

        // 配置 Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "His API",
                Version = "v1",
                Description = "His API 接口文档"
            });

            // 获取所有 XML 注释文件
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.AllDirectories);

            // 遍历所有 XML 文件并加载
            foreach (var xmlFile in xmlFiles)
            {
                options.IncludeXmlComments(xmlFile);
            }

            // 屏蔽所有包含 "abp" 的路由
            options.DocInclusionPredicate((docName, description) => !description.RelativePath.Contains("abp"));

            // JWT 认证配置
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

            // 添加冲突解析器
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

        });

        //services.AddSwaggerGenNewtonsoftSupport(); // 添加 Newtonsoft.Json 支持，如果需要的话

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

            // 添加事件处理
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
        services.AddScoped<CurrentUserService>();  // 为了中间件可以直接注入
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
        app.UseCors("SignalRCors");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ApiLoggingMiddleware>();

        // 添加当前用户中间件 - 在认证和授权之后
        app.UseCurrentUser();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "His API V1");
            c.EnableFilter(); // 添加搜索框
            c.DocExpansion(DocExpansion.None); // 默认折叠所有端点
            c.DefaultModelsExpandDepth(-1); // 隐藏模型部分，设置为 0 则显示但折叠

            // 添加自定义 CSS 以自定义 UI
            c.InjectStylesheet("/swagger-ui/custom.css");

            // 添加自定义 JavaScript 以控制 UI 行为
            c.InjectJavascript("/swagger-ui/custom.js");
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<qvzhongren.Message.Application.Hubs.ChatHub>("/hubs/chat");
        });
    }

    private  string GetParas(SugarParameter[] pars)
    {
        string key = string.Empty;
        if (pars.Length > 0)
        {
            key = "【SQL参数】";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}：{param.Value}\n";
            }
        }
        return key;
    }
}