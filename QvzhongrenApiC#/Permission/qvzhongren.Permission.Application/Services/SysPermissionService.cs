using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;
using qvzhongren.Repository.SqlSugar;

namespace qvzhongren.Permission.Application.Services;

/// <summary>
/// 权限管理服务
/// </summary>
public class SysPermissionService : CrudService<SysPermission, SysPermissionResponseDto, SysPermissionCreateDto>
{
    public SysPermissionService(IBaseRepository<SysPermission> repository, IMapper mapper)
        : base(repository, mapper)
    {
    }

    /// <summary>
    /// 获取所有权限列表
    /// </summary>
    [HttpPost("GetList")]
    public async Task<ResultDto<List<SysPermissionResponseDto>>> GetListAsync()
    {
        try
        {
            var list = await _repository.GetAllAsync();
            var result = _mapper.Map<List<SysPermission>, List<SysPermissionResponseDto>>(list);
            return ResultDto<List<SysPermissionResponseDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultDto<List<SysPermissionResponseDto>>.Error($"获取失败: {ex.Message}");
        }
    }
}
