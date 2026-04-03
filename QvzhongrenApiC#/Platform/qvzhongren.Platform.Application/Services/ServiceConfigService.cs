using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Platform.Application.Dtos;
using qvzhongren.Platform.Model;
using qvzhongren.Repository.SqlSugar;
using SqlSugar;

namespace qvzhongren.Platform.Application.Services;

/// <summary>
/// 服务配置管理
/// </summary>
public class ServiceConfigService : CrudService<SysServiceConfig, ServiceConfigResponseDto, ServiceConfigCreateDto>
{
    private readonly ISqlSugarClient _db;

    public ServiceConfigService(
        IBaseRepository<SysServiceConfig> repository,
        ISqlSugarClient db,
        IMapper mapper) : base(repository, mapper)
    {
        _db = db;
    }

    /// <summary>获取所有服务配置</summary>
    [HttpPost("GetList")]
    public async Task<ResultDto<List<ServiceConfigResponseDto>>> GetListAsync()
    {
        try
        {
            var list = await _db.Queryable<SysServiceConfig>()
                .OrderBy(c => c.SortNo)
                .ToListAsync();
            var result = _mapper.Map<List<ServiceConfigResponseDto>>(list);
            return ResultDto<List<ServiceConfigResponseDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return ResultDto<List<ServiceConfigResponseDto>>.Error($"查询失败: {ex.Message}");
        }
    }

    /// <summary>根据服务名称获取配置（供其他服务启动时调用）</summary>
    [AllowAnonymous]
    [HttpPost("GetByName")]
    public async Task<ResultDto<ServiceConfigResponseDto>> GetByNameAsync([FromBody] string serviceName)
    {
        try
        {
            var config = await _db.Queryable<SysServiceConfig>()
                .Where(c => c.ServiceName == serviceName && c.Status == "1")
                .FirstAsync();
            if (config == null)
                return ResultDto<ServiceConfigResponseDto>.Error("未找到服务配置");
            return ResultDto<ServiceConfigResponseDto>.Success(_mapper.Map<ServiceConfigResponseDto>(config));
        }
        catch (Exception ex)
        {
            return ResultDto<ServiceConfigResponseDto>.Error($"查询失败: {ex.Message}");
        }
    }

    /// <summary>获取所有启用的服务配置（供 Gateway 启动时调用）</summary>
    [AllowAnonymous]
    [HttpPost("GetAllActive")]
    public async Task<ResultDto<List<ServiceConfigResponseDto>>> GetAllActiveAsync()
    {
        try
        {
            var list = await _db.Queryable<SysServiceConfig>()
                .Where(c => c.Status == "1")
                .OrderBy(c => c.SortNo)
                .ToListAsync();
            return ResultDto<List<ServiceConfigResponseDto>>.Success(
                _mapper.Map<List<ServiceConfigResponseDto>>(list));
        }
        catch (Exception ex)
        {
            return ResultDto<List<ServiceConfigResponseDto>>.Error($"查询失败: {ex.Message}");
        }
    }
}
