using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Repository.SqlSugar;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;
using SqlSugar;

namespace qvzhongren.Shop.Application.Services
{
    /// <summary>
    /// 商品服务
    /// </summary>
    public class ProductService : BaseService
    {
        private readonly IBaseRepository<ShopProduct> _repository;
        private readonly IMapper _mapper;
        private readonly ISqlSugarClient _db;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductService(IBaseRepository<ShopProduct> repository, IMapper mapper, ISqlSugarClient db)
        {
            _repository = repository;
            _mapper = mapper;
            _db = db;
        }

        /// <summary>
        /// 分页查询商品
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>分页商品列表</returns>
        [HttpPost("GetPage")]
        [AllowAnonymous]
        public async Task<ResultDto<ListPageResultDto<ProductResponseDto>>> GetPageAsync([FromBody] ProductQueryDto query)
        {
            try
            {
                var totalCount = new RefAsync<int>();
                var list = await _db.Queryable<ShopProduct>()
                    .WhereIF(!string.IsNullOrEmpty(query.CategoryId), x => x.CategoryId == query.CategoryId)
                    .WhereIF(!string.IsNullOrEmpty(query.Keyword), x => x.ProductName.Contains(query.Keyword) || x.Description.Contains(query.Keyword))
                    .WhereIF(!string.IsNullOrEmpty(query.Status), x => x.Status == query.Status)
                    .OrderBy(x => x.SortNo)
                    .OrderBy(x => x.CreateDate, OrderByType.Desc)
                    .ToPageListAsync(query.PageIndex, query.PageSize, totalCount);

                var dtoList = _mapper.Map<List<ProductResponseDto>>(list);

                var result = new ListPageResultDto<ProductResponseDto>
                {
                    TotalCount = totalCount.Value,
                    PageIndex = query.PageIndex,
                    PageSize = query.PageSize,
                    Values = dtoList
                };

                return ResultDto<ListPageResultDto<ProductResponseDto>>.Success(result, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<ListPageResultDto<ProductResponseDto>>.Error($"获取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据ID获取商品详情
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品详情</returns>
        [HttpPost("GetById")]
        [AllowAnonymous]
        public async Task<ResultDto<ProductResponseDto>> GetByIdAsync([FromBody] string productId)
        {
            try
            {
                if (string.IsNullOrEmpty(productId))
                {
                    return ResultDto<ProductResponseDto>.BadRequest("商品ID不能为空");
                }

                var entity = await _repository.GetByIdAsync(productId);
                if (entity == null)
                {
                    return ResultDto<ProductResponseDto>.Error("商品不存在");
                }

                var result = _mapper.Map<ProductResponseDto>(entity);
                return ResultDto<ProductResponseDto>.Success(result, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<ProductResponseDto>.Error($"获取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建商品
        /// </summary>
        /// <param name="dto">商品信息</param>
        /// <returns>创建结果</returns>
        [HttpPost("Create")]
        public async Task<ResultDto<bool>> CreateAsync([FromBody] ProductCreateDto dto)
        {
            try
            {
                var entity = _mapper.Map<ShopProduct>(dto);
                if (string.IsNullOrEmpty(entity.ProductId))
                {
                    entity.ProductId = Guid.NewGuid().ToString("N");
                }
                entity.Sales = 0;
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
        /// 更新商品
        /// </summary>
        /// <param name="dto">商品信息</param>
        /// <returns>更新结果</returns>
        [HttpPost("Update")]
        public async Task<ResultDto<bool>> UpdateAsync([FromBody] ProductCreateDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.ProductId))
                {
                    return ResultDto<bool>.BadRequest("商品ID不能为空");
                }

                var entity = await _repository.GetByIdAsync(dto.ProductId);
                if (entity == null)
                {
                    return ResultDto<bool>.Error("商品不存在");
                }

                entity.CategoryId = dto.CategoryId;
                entity.ProductName = dto.ProductName;
                entity.Description = dto.Description;
                entity.Price = dto.Price;
                entity.OriginalPrice = dto.OriginalPrice;
                entity.Stock = dto.Stock;
                entity.MainImage = dto.MainImage;
                entity.Images = dto.Images;
                entity.Status = dto.Status;
                entity.SortNo = dto.SortNo;
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
        /// 删除商品
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>删除结果</returns>
        [HttpPost("Delete")]
        public async Task<ResultDto<bool>> DeleteAsync([FromBody] string productId)
        {
            try
            {
                if (string.IsNullOrEmpty(productId))
                {
                    return ResultDto<bool>.BadRequest("商品ID不能为空");
                }

                var result = await _repository.DeleteByIdAsync(productId);
                return result > 0
                    ? ResultDto<bool>.Success(true, "删除成功")
                    : ResultDto<bool>.Error("删除失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"删除失败: {ex.Message}");
            }
        }
    }
}
