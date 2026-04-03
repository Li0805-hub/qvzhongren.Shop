using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Platform.Application.Dtos;
using qvzhongren.Platform.Model;
using SqlSugar;

namespace qvzhongren.Platform.Application.Services;

/// <summary>
/// 系统日志服务
/// </summary>
public class LogService : BaseService
{
    private readonly ISqlSugarClient _db;

    public LogService(ISqlSugarClient db)
    {
        _db = db;
    }

    [HttpPost("GetPage")]
    public async Task<ResultDto<ListPageResultDto<SysLog>>> GetPageAsync([FromBody] LogQueryDto query)
    {
        try
        {
            var pageIndex = query.PageIndex < 1 ? 1 : query.PageIndex;
            var pageSize = query.PageSize < 1 ? 20 : query.PageSize;
            if (pageSize > 100) pageSize = 100;

            var total = new RefAsync<int>();
            var items = await _db.Queryable<SysLog>()
                .WhereIF(!string.IsNullOrEmpty(query.Type), x => x.Type == query.Type)
                .WhereIF(!string.IsNullOrEmpty(query.Keyword), x => x.Name.Contains(query.Keyword!) || x.Content.Contains(query.Keyword!))
                .WhereIF(query.StartTime.HasValue, x => x.Timestamp >= query.StartTime!.Value)
                .WhereIF(query.EndTime.HasValue, x => x.Timestamp <= query.EndTime!.Value)
                .OrderByDescending(x => x.Timestamp)
                .ToPageListAsync(pageIndex, pageSize, total);

            var dto = new ListPageResultDto<SysLog>
            {
                TotalCount = total.Value,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Values = items
            };

            return ResultDto<ListPageResultDto<SysLog>>.Success(dto);
        }
        catch (Exception ex)
        {
            return ResultDto<ListPageResultDto<SysLog>>.Error($"查询失败: {ex.Message}");
        }
    }

    /// <summary>按条件批量删除日志</summary>
    [HttpPost("DeleteBatch")]
    public async Task<ResultDto<int>> DeleteBatchAsync([FromBody] LogDeleteDto dto)
    {
        try
        {
            var deleteable = _db.Deleteable<SysLog>();

            if (dto.StartTime.HasValue)
                deleteable = deleteable.Where(x => x.Timestamp >= dto.StartTime.Value);
            if (dto.EndTime.HasValue)
                deleteable = deleteable.Where(x => x.Timestamp <= dto.EndTime.Value);
            if (!string.IsNullOrEmpty(dto.Type))
                deleteable = deleteable.Where(x => x.Type == dto.Type);
            if (!string.IsNullOrEmpty(dto.Keyword))
                deleteable = deleteable.Where(x => x.Name.Contains(dto.Keyword) || x.Content.Contains(dto.Keyword));

            // 安全检查：至少要有一个条件
            if (!dto.StartTime.HasValue && !dto.EndTime.HasValue && string.IsNullOrEmpty(dto.Type) && string.IsNullOrEmpty(dto.Keyword))
                return ResultDto<int>.Error("请至少指定一个删除条件");

            var count = await deleteable.ExecuteCommandAsync();
            return ResultDto<int>.Success(count, $"已删除 {count} 条日志");
        }
        catch (Exception ex)
        {
            return ResultDto<int>.Error($"删除失败: {ex.Message}");
        }
    }
}
