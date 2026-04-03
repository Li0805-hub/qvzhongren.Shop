using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Agent.Application.Dtos;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Contracts.Clients;
using qvzhongren.Shared.Helper;
using SqlSugar;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace qvzhongren.Agent.Application.Services;

/// <summary>
/// AI 智能助手服务
/// </summary>
public class AgentService : BaseService
{
    private readonly IPermissionApiClient _permissionApi;
    private readonly IPlatformApiClient _platformApi;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISqlSugarClient _db;

    private static readonly string SystemPrompt = @"你是曲终人管理系统的智能助手。你可以帮助用户：
1. 查询系统数据（用户、角色、菜单、日志等）
2. 导航到系统页面
3. 回答关于系统的问题

可用页面：
- /dashboard - 仪表盘
- /system/users - 用户管理
- /system/roles - 角色管理
- /system/menus - 菜单管理
- /system/permissions - 权限管理
- /system/depts - 部门管理
- /system/logs - 系统日志

请用中文回复，回答要简洁有用。当用户要求查询数据时，使用工具获取真实数据后再回复。";

    private static readonly JsonElement Tools;

    static AgentService()
    {
        var toolsJson = @"[
  {
    ""type"": ""function"",
    ""function"": {
      ""name"": ""navigate_to_page"",
      ""description"": ""导航到系统中的指定页面"",
      ""parameters"": {
        ""type"": ""object"",
        ""properties"": {
          ""path"": { ""type"": ""string"", ""description"": ""页面路径"" }
        },
        ""required"": [""path""]
      }
    }
  },
  {
    ""type"": ""function"",
    ""function"": {
      ""name"": ""query_users"",
      ""description"": ""查询系统中的所有用户列表"",
      ""parameters"": { ""type"": ""object"", ""properties"": {} }
    }
  },
  {
    ""type"": ""function"",
    ""function"": {
      ""name"": ""query_roles"",
      ""description"": ""查询系统中的所有角色列表"",
      ""parameters"": { ""type"": ""object"", ""properties"": {} }
    }
  },
  {
    ""type"": ""function"",
    ""function"": {
      ""name"": ""query_menus"",
      ""description"": ""查询系统中的所有菜单列表"",
      ""parameters"": { ""type"": ""object"", ""properties"": {} }
    }
  },
  {
    ""type"": ""function"",
    ""function"": {
      ""name"": ""query_logs"",
      ""description"": ""查询系统日志"",
      ""parameters"": {
        ""type"": ""object"",
        ""properties"": {
          ""type"": { ""type"": ""string"", ""description"": ""日志类型"" },
          ""count"": { ""type"": ""integer"", ""description"": ""返回条数，默认10"" }
        }
      }
    }
  },
  {
    ""type"": ""function"",
    ""function"": {
      ""name"": ""query_statistics"",
      ""description"": ""查询系统统计概览"",
      ""parameters"": { ""type"": ""object"", ""properties"": {} }
    }
  }
]";
        Tools = JsonSerializer.Deserialize<JsonElement>(toolsJson);
    }

    public AgentService(
        IPermissionApiClient permissionApi,
        IPlatformApiClient platformApi,
        IHttpClientFactory httpClientFactory,
        ISqlSugarClient db)
    {
        _permissionApi = permissionApi;
        _platformApi = platformApi;
        _httpClientFactory = httpClientFactory;
        _db = db;
    }

    /// <summary>从数据库读取配置，fallback 到 appsettings</summary>
    private string GetConfig(string name, string fallbackKey)
    {
        try
        {
            var val = _db.Queryable<object>().AS("SYS_SERVICE_CONFIG")
                .Where("\"SERVICE_NAME\" = @name AND \"STATUS\" = '1'", new { name })
                .Select<string>("\"SERVICE_URL\"").First();
            if (!string.IsNullOrEmpty(val)) return val;
        }
        catch { }
        return AppSettings.GetValue(fallbackKey) ?? "";
    }

    [HttpPost("Chat")]
    [AllowAnonymous]
    public async Task<ResultDto<AgentResponse>> ChatAsync([FromBody] AgentRequest request)
    {
        try
        {
            var apiKey = GetConfig("DeepSeek:ApiKey", "DeepSeek:ApiKey");
            var baseUrl = GetConfig("DeepSeek:BaseUrl", "DeepSeek:BaseUrl");
            var model = GetConfig("DeepSeek:Model", "DeepSeek:Model");

            var messages = new List<object>();
            messages.Add(new { role = "system", content = SystemPrompt });
            foreach (var msg in request.Messages)
            {
                messages.Add(new { role = msg.Role, content = msg.Content });
            }

            var response = await CallDeepSeek(apiKey, baseUrl, model, messages);

            var toolCalls = new List<AgentToolCall>();
            string? navigateTo = null;

            int maxIterations = 5;
            while (response.Choices?[0]?.Message?.ToolCalls != null && maxIterations-- > 0)
            {
                var assistantMsg = response.Choices[0].Message;
                var assistantToolCalls = assistantMsg.ToolCalls!.Select(tc => new
                {
                    id = tc.Id,
                    type = "function",
                    function = new { name = tc.Function.Name, arguments = tc.Function.Arguments }
                }).ToList();
                messages.Add(new
                {
                    role = "assistant",
                    content = assistantMsg.Content ?? (object)null!,
                    tool_calls = assistantToolCalls
                });

                foreach (var tc in assistantMsg.ToolCalls)
                {
                    var result = await ExecuteTool(tc.Function.Name, tc.Function.Arguments);

                    if (tc.Function.Name == "navigate_to_page")
                    {
                        var args = JsonSerializer.Deserialize<JsonElement>(tc.Function.Arguments);
                        if (args.TryGetProperty("path", out var pathEl))
                            navigateTo = pathEl.GetString();
                    }

                    toolCalls.Add(new AgentToolCall
                    {
                        Name = tc.Function.Name,
                        Arguments = tc.Function.Arguments,
                        Result = result
                    });

                    messages.Add(new { role = "tool", tool_call_id = tc.Id, content = result });
                }

                response = await CallDeepSeek(apiKey, baseUrl, model, messages);
            }

            var content = response.Choices?[0]?.Message?.Content ?? "抱歉，我无法处理你的请求。";

            return ResultDto<AgentResponse>.Success(new AgentResponse
            {
                Content = content,
                NavigateTo = navigateTo,
                ToolCalls = toolCalls.Count > 0 ? toolCalls : null
            });
        }
        catch (Exception ex)
        {
            return ResultDto<AgentResponse>.Error($"智能助手错误: {ex.Message}");
        }
    }

    private async Task<DeepSeekResponse> CallDeepSeek(string apiKey, string baseUrl, string model, List<object> messages)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var body = new
        {
            model = model,
            messages = messages,
            tools = Tools,
            tool_choice = "auto"
        };

        var json = JsonSerializer.Serialize(body, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var resp = await client.PostAsync($"{baseUrl}/chat/completions", content);
        var respBytes = await resp.Content.ReadAsByteArrayAsync();
        var respBody = new UTF8Encoding(false, false).GetString(respBytes);

        if (!resp.IsSuccessStatusCode)
            throw new Exception($"DeepSeek API error ({resp.StatusCode}): {respBody}");

        return JsonSerializer.Deserialize<DeepSeekResponse>(respBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        })!;
    }

    private async Task<string> ExecuteTool(string name, string arguments)
    {
        try
        {
            var opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            switch (name)
            {
                case "navigate_to_page":
                    var navArgs = JsonSerializer.Deserialize<JsonElement>(arguments);
                    var path = navArgs.GetProperty("path").GetString();
                    return $"已导航到 {path}";

                case "query_users":
                    var users = await _permissionApi.GetAllUsersAsync();
                    return JsonSerializer.Serialize(users, opts);

                case "query_roles":
                    var roles = await _permissionApi.GetAllRolesAsync();
                    return JsonSerializer.Serialize(roles, opts);

                case "query_menus":
                    var menus = await _permissionApi.GetAllMenusAsync();
                    return JsonSerializer.Serialize(menus, opts);

                case "query_logs":
                    var logArgs = JsonSerializer.Deserialize<JsonElement>(arguments);
                    var logType = logArgs.TryGetProperty("type", out var t) ? t.GetString() : null;
                    var count = logArgs.TryGetProperty("count", out var c) ? c.GetInt32() : 10;
                    var logs = await _platformApi.GetRecentLogsAsync(logType, count);
                    return JsonSerializer.Serialize(logs, opts);

                case "query_statistics":
                    var userList = await _permissionApi.GetAllUsersAsync();
                    var roleList = await _permissionApi.GetAllRolesAsync();
                    var menuList = await _permissionApi.GetAllMenusAsync();
                    var logCount = await _platformApi.GetLogCountAsync();
                    return JsonSerializer.Serialize(new
                    {
                        userCount = userList.Count,
                        roleCount = roleList.Count,
                        menuCount = menuList.Count,
                        logCount
                    });

                default:
                    return $"未知工具: {name}";
            }
        }
        catch (Exception ex)
        {
            return $"工具执行错误: {ex.Message}";
        }
    }
}
