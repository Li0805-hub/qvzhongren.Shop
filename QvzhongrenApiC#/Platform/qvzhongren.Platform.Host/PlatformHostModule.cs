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
using qvzhongren.Platform.Application;
using qvzhongren.Platform.Application.Services;
using qvzhongren.Application.Common;
using qvzhongren.Application.Interceptors;
using qvzhongren.Application.Logging;
using qvzhongren.Contracts.Clients;
using qvzhongren.Repository.SqlSugar;
using qvzhongren.Shared.Common;
using qvzhongren.Shared.Helper;

namespace qvzhongren.Platform.Host;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(PlatformApplicationModule)
)]
public class PlatformHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        var configuration = context.Services.GetConfiguration();

        // 注册 AppSettings
        services.AddSingleton(new AppSettings(configuration));

        // 注册 LoggingInterceptor
        services.AddTransient<LoggingInterceptor>();

        // 注册 DefaultLogWriter 作为 ILogWriter
        services.AddScoped<ILogWriter, DefaultLogWriter>();

        // 注册日志
        services.AddScoped<Logger>();

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

        services.AddAutoMapper(typeof(PlatformHostModule));

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
            options.ConventionalControllers.Create(typeof(PlatformHostModule).Assembly);
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
                Title = "Platform API",
                Version = "v1",
                Description = "Platform 微服务接口文档"
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

        // 支持文件上传的静态文件服务
        app.UseStaticFiles();

        app.UseCors("AllowAll");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Platform API V1");
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
