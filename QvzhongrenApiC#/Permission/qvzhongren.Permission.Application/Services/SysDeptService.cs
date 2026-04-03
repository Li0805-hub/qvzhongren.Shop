using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Permission.Application.Dtos;
using qvzhongren.Permission.Model;
using SqlSugar;

namespace qvzhongren.Permission.Application.Services;

/// <summary>
/// 部门管理服务
/// </summary>
public class SysDeptService : BaseService
{
    private readonly ISqlSugarClient _db;

    public SysDeptService(ISqlSugarClient db)
    {
        _db = db;
    }

    [HttpPost("GetList")]
    public async Task<ResultDto<List<SysDept>>> GetListAsync()
    {
        try
        {
            var list = await _db.Queryable<SysDept>().OrderBy(d => d.SortNo).ToListAsync();
            return ResultDto<List<SysDept>>.Success(list);
        }
        catch (Exception ex)
        {
            return ResultDto<List<SysDept>>.Error($"查询失败: {ex.Message}");
        }
    }

    [HttpPost("GetTree")]
    public async Task<ResultDto<List<DeptTreeDto>>> GetTreeAsync()
    {
        try
        {
            var list = await _db.Queryable<SysDept>().OrderBy(d => d.SortNo).ToListAsync();
            var tree = BuildTree(list, "0");
            return ResultDto<List<DeptTreeDto>>.Success(tree);
        }
        catch (Exception ex)
        {
            return ResultDto<List<DeptTreeDto>>.Error($"查询失败: {ex.Message}");
        }
    }

    [HttpPost("Create")]
    public async Task<ResultDto<bool>> CreateAsync([FromBody] DeptCreateDto dto)
    {
        try
        {
            var entity = new SysDept
            {
                DeptCode = dto.DeptCode,
                DeptName = dto.DeptName,
                ParentCode = dto.ParentCode ?? "0",
                SortNo = dto.SortNo,
                Status = dto.Status ?? "1",
                Leader = dto.Leader,
                Phone = dto.Phone,
                CreateCode = "admin",
                CreateDate = DateTime.Now,
                UpdateCode = "admin",
                UpdateDate = DateTime.Now,
            };
            await _db.Insertable(entity).ExecuteCommandAsync();
            return ResultDto<bool>.Success(true, "创建成功");
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"创建失败: {ex.Message}");
        }
    }

    [HttpPost("Update")]
    public async Task<ResultDto<bool>> UpdateAsync([FromBody] DeptCreateDto dto)
    {
        try
        {
            var entity = await _db.Queryable<SysDept>().FirstAsync(d => d.DeptCode == dto.DeptCode);
            if (entity == null) return ResultDto<bool>.Error("部门不存在");
            entity.DeptName = dto.DeptName;
            entity.ParentCode = dto.ParentCode ?? entity.ParentCode;
            entity.SortNo = dto.SortNo;
            entity.Status = dto.Status ?? entity.Status;
            entity.Leader = dto.Leader;
            entity.Phone = dto.Phone;
            entity.UpdateCode = "admin";
            entity.UpdateDate = DateTime.Now;
            await _db.Updateable(entity).ExecuteCommandAsync();
            return ResultDto<bool>.Success(true, "更新成功");
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"更新失败: {ex.Message}");
        }
    }

    [HttpPost("Delete")]
    public async Task<ResultDto<bool>> DeleteAsync([FromBody] string deptCode)
    {
        try
        {
            var children = await _db.Queryable<SysDept>().Where(d => d.ParentCode == deptCode).CountAsync();
            if (children > 0) return ResultDto<bool>.Error("请先删除子部门");
            await _db.Deleteable<SysDept>().Where(d => d.DeptCode == deptCode).ExecuteCommandAsync();
            return ResultDto<bool>.Success(true, "删除成功");
        }
        catch (Exception ex)
        {
            return ResultDto<bool>.Error($"删除失败: {ex.Message}");
        }
    }

    private List<DeptTreeDto> BuildTree(List<SysDept> all, string parentCode)
    {
        return all.Where(d => d.ParentCode == parentCode)
            .Select(d => new DeptTreeDto
            {
                DeptCode = d.DeptCode,
                DeptName = d.DeptName,
                ParentCode = d.ParentCode,
                SortNo = d.SortNo,
                Status = d.Status,
                Leader = d.Leader,
                Phone = d.Phone,
                CreateDate = d.CreateDate,
                Children = BuildTree(all, d.DeptCode) is { Count: > 0 } c ? c : null,
            }).ToList();
    }
}
