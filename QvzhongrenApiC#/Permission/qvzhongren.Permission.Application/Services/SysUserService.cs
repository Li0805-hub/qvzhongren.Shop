using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;
using qvzhongren.Repository.SqlSugar;

namespace qvzhongren.Permission.Application.Services;

/// <summary>
/// 用户管理服务
/// </summary>
public class SysUserService : CrudService<SysUser, SysUserResponseDto, SysUserCreateDto>
{
    private readonly IBaseRepository<SysUserRole> _userRoleRepo;
    private readonly IBaseRepository<SysUserMenu> _userMenuRepo;

    public SysUserService(
        IBaseRepository<SysUser> repository,
        IBaseRepository<SysUserRole> userRoleRepo,
        IBaseRepository<SysUserMenu> userMenuRepo,
        IMapper mapper) : base(repository, mapper)
    {
        _userRoleRepo = userRoleRepo;
        _userMenuRepo = userMenuRepo;
    }

    /// <summary>
    /// 获取所有用户列表
    /// </summary>
    [HttpPost("GetList")]
    public async Task<ResultDto<List<SysUserResponseDto>>> GetListAsync()
    {
        try
        {
            var list = await _repository.GetAllAsync();
            var result = _mapper.Map<List<SysUser>, List<SysUserResponseDto>>(list);
            return ResultDto<List<SysUserResponseDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultDto<List<SysUserResponseDto>>.Error($"获取失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 为用户分配角色
    /// </summary>
    [HttpPost("AssignRoles")]
    public async Task<ResultDto<bool>> AssignRolesAsync([FromBody] UserRoleAssignDto dto)
    {
        try
        {
            // 删除用户原有角色
            await _userRoleRepo.DeleteAsync(x => x.UserId == dto.UserId);

            // 添加新角色
            if (dto.RoleIds != null && dto.RoleIds.Any())
            {
                var userRoles = dto.RoleIds.Select(roleId => new SysUserRole
                {
                    UserId = dto.UserId,
                    RoleId = roleId
                }).ToList();
                await _userRoleRepo.InsertAsync(userRoles);
            }

            return ResultDto<bool>.Success(true, "角色分配成功");
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"角色分配失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 获取用户的角色ID列表
    /// </summary>
    [HttpPost("GetUserRoles")]
    public async Task<ResultDto<List<string>>> GetUserRolesAsync([FromBody] string userId)
    {
        try
        {
            var userRoles = await _userRoleRepo.GetListAsync(x => x.UserId == userId);
            var roleIds = userRoles.Select(x => x.RoleId).ToList();
            return ResultDto<List<string>>.Success(roleIds);
        }
        catch (Exception ex)
        {
            return ResultDto<List<string>>.Error($"获取失败: {ex.Message}");
        }
    }
    /// <summary>
    /// 为用户直接分配菜单权限
    /// </summary>
    [HttpPost("AssignMenus")]
    public async Task<ResultDto<bool>> AssignMenusAsync([FromBody] UserMenuAssignDto dto)
    {
        try
        {
            await _userMenuRepo.DeleteAsync(x => x.UserId == dto.UserId);

            if (dto.MenuIds != null && dto.MenuIds.Any())
            {
                var userMenus = dto.MenuIds.Select(menuId => new SysUserMenu
                {
                    UserId = dto.UserId,
                    MenuId = menuId
                }).ToList();
                await _userMenuRepo.InsertAsync(userMenus);
            }

            return ResultDto<bool>.Success(true, "用户菜单权限分配成功");
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"用户菜单权限分配失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 获取用户直接分配的菜单ID列表
    /// </summary>
    [HttpPost("GetUserMenus")]
    public async Task<ResultDto<List<string>>> GetUserMenusAsync([FromBody] string userId)
    {
        try
        {
            var userMenus = await _userMenuRepo.GetListAsync(x => x.UserId == userId);
            var menuIds = userMenus.Select(x => x.MenuId).ToList();
            return ResultDto<List<string>>.Success(menuIds);
        }
        catch (Exception ex)
        {
            return ResultDto<List<string>>.Error($"获取失败: {ex.Message}");
        }
    }
}

/// <summary>
/// 用户菜单分配DTO
/// </summary>
public class UserMenuAssignDto
{
    public string UserId { get; set; }
    public List<string> MenuIds { get; set; }
}

/// <summary>
/// 用户角色分配DTO
/// </summary>
public class UserRoleAssignDto
{
    public string UserId { get; set; }
    public List<string> RoleIds { get; set; }
}
