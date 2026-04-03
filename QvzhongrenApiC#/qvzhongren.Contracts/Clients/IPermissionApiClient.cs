using qvzhongren.Contracts.Dtos;

namespace qvzhongren.Contracts.Clients;

/// <summary>
/// Permission 服务的远程调用接口
/// </summary>
public interface IPermissionApiClient
{
    Task<List<SimpleUserDto>> GetAllUsersAsync();
    Task<List<SimpleUserDto>> GetActiveUsersAsync(string excludeUserId);
    Task<List<SimpleRoleDto>> GetAllRolesAsync();
    Task<List<SimpleMenuDto>> GetAllMenusAsync();
}
