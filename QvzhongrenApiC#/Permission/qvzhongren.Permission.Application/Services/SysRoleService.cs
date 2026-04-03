using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;
using qvzhongren.Repository.SqlSugar;

namespace qvzhongren.Permission.Application.Services;

/// <summary>
/// 角色管理服务
/// </summary>
public class SysRoleService : CrudService<SysRole, SysRoleResponseDto, SysRoleCreateDto>
{
    private readonly IBaseRepository<SysRoleMenu> _roleMenuRepo;

    public SysRoleService(
        IBaseRepository<SysRole> repository,
        IBaseRepository<SysRoleMenu> roleMenuRepo,
        IMapper mapper) : base(repository, mapper)
    {
        _roleMenuRepo = roleMenuRepo;
    }

    /// <summary>
    /// 获取所有角色列表
    /// </summary>
    [HttpPost("GetList")]
    public async Task<ResultDto<List<SysRoleResponseDto>>> GetListAsync()
    {
        try
        {
            var list = await _repository.GetAllAsync();
            var result = _mapper.Map<List<SysRole>, List<SysRoleResponseDto>>(list);
            return ResultDto<List<SysRoleResponseDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultDto<List<SysRoleResponseDto>>.Error($"获取失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 为角色分配菜单
    /// </summary>
    [HttpPost("AssignMenus")]
    public async Task<ResultDto<bool>> AssignMenusAsync([FromBody] RoleMenuAssignDto dto)
    {
        try
        {
            await _roleMenuRepo.DeleteAsync(x => x.RoleId == dto.RoleId);

            if (dto.MenuIds != null && dto.MenuIds.Any())
            {
                var roleMenus = dto.MenuIds.Select(menuId => new SysRoleMenu
                {
                    RoleId = dto.RoleId,
                    MenuId = menuId
                }).ToList();
                await _roleMenuRepo.InsertAsync(roleMenus);
            }

            return ResultDto<bool>.Success(true, "菜单分配成功");
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"菜单分配失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 获取角色的菜单ID列表
    /// </summary>
    [HttpPost("GetRoleMenus")]
    public async Task<ResultDto<List<string>>> GetRoleMenusAsync([FromBody] string roleId)
    {
        try
        {
            var roleMenus = await _roleMenuRepo.GetListAsync(x => x.RoleId == roleId);
            var menuIds = roleMenus.Select(x => x.MenuId).ToList();
            return ResultDto<List<string>>.Success(menuIds);
        }
        catch (Exception ex)
        {
            return ResultDto<List<string>>.Error($"获取失败: {ex.Message}");
        }
    }
}

/// <summary>
/// 角色菜单分配DTO
/// </summary>
public class RoleMenuAssignDto
{
    public string RoleId { get; set; }
    public List<string> MenuIds { get; set; }
}
