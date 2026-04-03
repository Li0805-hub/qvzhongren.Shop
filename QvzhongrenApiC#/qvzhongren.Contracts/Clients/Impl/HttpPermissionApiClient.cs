using System.Net.Http.Json;
using qvzhongren.Contracts.Dtos;

namespace qvzhongren.Contracts.Clients.Impl;

/// <summary>
/// Permission 服务的 HTTP 远程调用实现（分布式模式使用）
/// </summary>
public class HttpPermissionApiClient : IPermissionApiClient
{
    private readonly HttpClient _http;

    public HttpPermissionApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<SimpleUserDto>> GetAllUsersAsync()
    {
        var result = await _http.PostAsync("api/Permission/SysUser/GetList", null);
        var response = await result.Content.ReadFromJsonAsync<ApiResult<List<SimpleUserDto>>>();
        return response?.Data ?? new List<SimpleUserDto>();
    }

    public async Task<List<SimpleUserDto>> GetActiveUsersAsync(string excludeUserId)
    {
        var all = await GetAllUsersAsync();
        return all.Where(u => u.UserId != excludeUserId && u.Status == "1").ToList();
    }

    public async Task<List<SimpleRoleDto>> GetAllRolesAsync()
    {
        var result = await _http.PostAsync("api/Permission/SysRole/GetList", null);
        var response = await result.Content.ReadFromJsonAsync<ApiResult<List<SimpleRoleDto>>>();
        return response?.Data ?? new List<SimpleRoleDto>();
    }

    public async Task<List<SimpleMenuDto>> GetAllMenusAsync()
    {
        var result = await _http.PostAsync("api/Permission/SysMenu/GetList", null);
        var response = await result.Content.ReadFromJsonAsync<ApiResult<List<SimpleMenuDto>>>();
        return response?.Data ?? new List<SimpleMenuDto>();
    }
}
