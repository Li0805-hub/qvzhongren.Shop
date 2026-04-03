using qvzhongren.Contracts.Dtos;
using SqlSugar;

namespace qvzhongren.Contracts.Clients.Impl;

/// <summary>
/// Permission 服务的本地直连实现（单体模式使用，直接查数据库）
/// </summary>
public class LocalPermissionApiClient : IPermissionApiClient
{
    private readonly ISqlSugarClient _db;

    public LocalPermissionApiClient(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<List<SimpleUserDto>> GetAllUsersAsync()
    {
        return await _db.Queryable<object>()
            .AS("SYS_USER")
            .Where("\"STATUS\" = '1' OR \"STATUS\" IS NOT NULL")
            .Select<SimpleUserDto>("\"USER_ID\" as UserId, \"USER_NAME\" as UserName, \"REAL_NAME\" as RealName, \"PHONE\" as Phone, \"EMAIL\" as Email, \"DEPT_CODE\" as DeptCode, \"STATUS\" as Status")
            .ToListAsync();
    }

    public async Task<List<SimpleUserDto>> GetActiveUsersAsync(string excludeUserId)
    {
        return await _db.Queryable<object>()
            .AS("SYS_USER")
            .Where($"\"USER_ID\" != '{excludeUserId}' AND \"STATUS\" = '1'")
            .Select<SimpleUserDto>("\"USER_ID\" as UserId, \"USER_NAME\" as UserName, \"REAL_NAME\" as RealName, \"PHONE\" as Phone, \"EMAIL\" as Email, \"DEPT_CODE\" as DeptCode, \"STATUS\" as Status")
            .ToListAsync();
    }

    public async Task<List<SimpleRoleDto>> GetAllRolesAsync()
    {
        return await _db.Queryable<object>()
            .AS("SYS_ROLE")
            .Select<SimpleRoleDto>("\"ROLE_ID\" as RoleId, \"ROLE_CODE\" as RoleCode, \"ROLE_NAME\" as RoleName, \"DESCRIPTION\" as Description, \"STATUS\" as Status")
            .ToListAsync();
    }

    public async Task<List<SimpleMenuDto>> GetAllMenusAsync()
    {
        return await _db.Queryable<object>()
            .AS("SYS_MENU")
            .Select<SimpleMenuDto>("\"MENU_ID\" as MenuId, \"MENU_NAME\" as MenuName, \"PATH\" as Path, \"MENU_TYPE\" as MenuType, \"STATUS\" as Status, \"PARENT_ID\" as ParentId")
            .ToListAsync();
    }
}
