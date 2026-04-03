using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;
using qvzhongren.Repository.SqlSugar;

namespace qvzhongren.Permission.Application.Services;

/// <summary>
/// 菜单管理服务
/// </summary>
public class SysMenuService : CrudService<SysMenu, SysMenuResponseDto, SysMenuCreateDto>
{
    public SysMenuService(IBaseRepository<SysMenu> repository, IMapper mapper)
        : base(repository, mapper)
    {
    }

    /// <summary>
    /// 获取所有菜单列表
    /// </summary>
    [HttpPost("GetList")]
    public async Task<ResultDto<List<SysMenuResponseDto>>> GetListAsync()
    {
        try
        {
            var list = await _repository.GetAllAsync();
            var result = _mapper.Map<List<SysMenu>, List<SysMenuResponseDto>>(list);
            return ResultDto<List<SysMenuResponseDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultDto<List<SysMenuResponseDto>>.Error($"获取失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 获取树形菜单
    /// </summary>
    [HttpPost("GetMenuTree")]
    public async Task<ResultDto<List<SysMenuResponseDto>>> GetMenuTreeAsync()
    {
        try
        {
            var allMenus = await _repository.GetAllAsync();
            var menuDtos = _mapper.Map<List<SysMenu>, List<SysMenuResponseDto>>(allMenus);
            var tree = BuildTree(menuDtos, "0");
            return ResultDto<List<SysMenuResponseDto>>.Success(tree);
        }
        catch (Exception ex)
        {
            return ResultDto<List<SysMenuResponseDto>>.Error($"获取失败: {ex.Message}");
        }
    }

    private List<SysMenuResponseDto> BuildTree(List<SysMenuResponseDto> allMenus, string parentId)
    {
        return allMenus
            .Where(m => m.ParentId == parentId)
            .OrderBy(m => m.SortNo)
            .Select(m =>
            {
                m.Children = BuildTree(allMenus, m.MenuId);
                if (m.Children.Count == 0) m.Children = null;
                return m;
            })
            .ToList();
    }
}
