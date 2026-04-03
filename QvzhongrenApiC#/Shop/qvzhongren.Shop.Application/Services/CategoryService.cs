using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Repository.SqlSugar;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;

namespace qvzhongren.Shop.Application.Services
{
    /// <summary>
    /// 商品分类服务
    /// </summary>
    public class CategoryService : BaseService
    {
        private readonly IBaseRepository<ShopCategory> _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CategoryService(IBaseRepository<ShopCategory> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取分类列表（平铺）
        /// </summary>
        /// <returns>分类列表</returns>
        [HttpPost("GetList")]
        [AllowAnonymous]
        public async Task<ResultDto<List<CategoryResponseDto>>> GetListAsync()
        {
            try
            {
                var list = await _repository.GetAllAsync();
                var result = _mapper.Map<List<CategoryResponseDto>>(list.OrderBy(x => x.SortNo).ToList());
                return ResultDto<List<CategoryResponseDto>>.Success(result, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<List<CategoryResponseDto>>.Error($"获取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取分类树形结构
        /// </summary>
        /// <returns>树形分类列表</returns>
        [HttpPost("GetTree")]
        [AllowAnonymous]
        public async Task<ResultDto<List<CategoryResponseDto>>> GetTreeAsync()
        {
            try
            {
                var list = await _repository.GetAllAsync();
                var dtoList = _mapper.Map<List<CategoryResponseDto>>(list.OrderBy(x => x.SortNo).ToList());
                var tree = BuildTree(dtoList, "0");
                return ResultDto<List<CategoryResponseDto>>.Success(tree, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<List<CategoryResponseDto>>.Error($"获取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="dto">分类信息</param>
        /// <returns>创建结果</returns>
        [HttpPost("Create")]
        public async Task<ResultDto<bool>> CreateAsync([FromBody] CategoryCreateDto dto)
        {
            try
            {
                var entity = _mapper.Map<ShopCategory>(dto);
                if (string.IsNullOrEmpty(entity.CategoryId))
                {
                    entity.CategoryId = Guid.NewGuid().ToString("N");
                }
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;

                var result = await _repository.InsertAsync(entity);
                return result > 0
                    ? ResultDto<bool>.Success(true, "创建成功")
                    : ResultDto<bool>.Error("创建失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"创建失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="dto">分类信息</param>
        /// <returns>更新结果</returns>
        [HttpPost("Update")]
        public async Task<ResultDto<bool>> UpdateAsync([FromBody] CategoryCreateDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.CategoryId))
                {
                    return ResultDto<bool>.BadRequest("分类ID不能为空");
                }

                var entity = await _repository.GetByIdAsync(dto.CategoryId);
                if (entity == null)
                {
                    return ResultDto<bool>.Error("分类不存在");
                }

                entity.ParentId = dto.ParentId;
                entity.CategoryName = dto.CategoryName;
                entity.Icon = dto.Icon;
                entity.SortNo = dto.SortNo;
                entity.Status = dto.Status;
                entity.UpdateDate = DateTime.Now;

                var result = await _repository.UpdateAsync(entity);
                return result > 0
                    ? ResultDto<bool>.Success(true, "更新成功")
                    : ResultDto<bool>.Error("更新失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"更新失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="categoryId">分类ID</param>
        /// <returns>删除结果</returns>
        [HttpPost("Delete")]
        public async Task<ResultDto<bool>> DeleteAsync([FromBody] string categoryId)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryId))
                {
                    return ResultDto<bool>.BadRequest("分类ID不能为空");
                }

                // 检查是否有子分类
                var children = await _repository.GetListAsync(x => x.ParentId == categoryId);
                if (children != null && children.Count > 0)
                {
                    return ResultDto<bool>.Error("该分类下存在子分类，无法删除");
                }

                var result = await _repository.DeleteByIdAsync(categoryId);
                return result > 0
                    ? ResultDto<bool>.Success(true, "删除成功")
                    : ResultDto<bool>.Error("删除失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"删除失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 递归构建树形结构
        /// </summary>
        private List<CategoryResponseDto> BuildTree(List<CategoryResponseDto> allNodes, string parentId)
        {
            var children = allNodes.Where(x => x.ParentId == parentId).ToList();
            foreach (var child in children)
            {
                child.Children = BuildTree(allNodes, child.CategoryId);
                if (child.Children.Count == 0)
                {
                    child.Children = null;
                }
            }
            return children;
        }
    }
}
