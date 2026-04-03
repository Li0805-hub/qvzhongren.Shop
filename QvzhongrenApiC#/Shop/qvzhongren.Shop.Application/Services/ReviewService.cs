using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using qvzhongren.Application;
using qvzhongren.Application.Dtos;
using qvzhongren.Repository.SqlSugar;
using qvzhongren.Shared.Common;
using qvzhongren.Shop.Application.Dtos;
using qvzhongren.Shop.Model;
using SqlSugar;

namespace qvzhongren.Shop.Application.Services
{
    /// <summary>
    /// 商品评价服务
    /// </summary>
    public class ReviewService : BaseService
    {
        private readonly IBaseRepository<ShopReview> _repository;
        private readonly IMapper _mapper;
        private readonly ISqlSugarClient _db;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReviewService(
            IBaseRepository<ShopReview> repository,
            IMapper mapper,
            ISqlSugarClient db,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _db = db;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// 创建商品评价
        /// </summary>
        /// <param name="dto">评价信息</param>
        /// <returns>创建结果</returns>
        [HttpPost("Create")]
        public async Task<ResultDto<bool>> CreateAsync([FromBody] ReviewCreateDto dto)
        {
            try
            {
                var userId = _currentUserService.User?.UserCode;
                if (string.IsNullOrEmpty(userId))
                {
                    return ResultDto<bool>.Error("未获取到当前用户信息", 401);
                }

                // 检查是否已评价
                var existing = await _repository.GetFirstAsync(
                    x => x.OrderId == dto.OrderId && x.ProductId == dto.ProductId && x.UserId == userId);
                if (existing != null)
                {
                    return ResultDto<bool>.Error("该商品已评价");
                }

                var entity = _mapper.Map<ShopReview>(dto);
                entity.ReviewId = Guid.NewGuid().ToString("N");
                entity.UserId = userId;
                entity.CreateCode = userId;
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;

                var result = await _repository.InsertAsync(entity);
                return result > 0
                    ? ResultDto<bool>.Success(true, "评价成功")
                    : ResultDto<bool>.Error("评价失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"评价失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取商品评价列表（关联用户名称）
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>评价列表</returns>
        [HttpPost("GetByProduct")]
        [AllowAnonymous]
        public async Task<ResultDto<List<ReviewResponseDto>>> GetByProductAsync([FromBody] string productId)
        {
            try
            {
                if (string.IsNullOrEmpty(productId))
                {
                    return ResultDto<List<ReviewResponseDto>>.BadRequest("商品ID不能为空");
                }

                var list = await _db.Ado.SqlQueryAsync<ReviewResponseDto>(
                    @"SELECT r.""REVIEW_ID"" AS ""ReviewId"", r.""ORDER_ID"" AS ""OrderId"", r.""PRODUCT_ID"" AS ""ProductId"",
                             r.""USER_ID"" AS ""UserId"", r.""RATING"" AS ""Rating"", r.""CONTENT"" AS ""Content"",
                             r.""IMAGES"" AS ""Images"", r.""CREATE_CODE"" AS ""CreateCode"", r.""CREATE_DATE"" AS ""CreateDate"",
                             u.""REAL_NAME"" AS ""UserName""
                      FROM ""SHOP_REVIEW"" r
                      LEFT JOIN ""SYS_USER"" u ON r.""USER_ID"" = u.""USER_ID""
                      WHERE r.""PRODUCT_ID"" = @ProductId
                      ORDER BY r.""CREATE_DATE"" DESC",
                    new { ProductId = productId });

                return ResultDto<List<ReviewResponseDto>>.Success(list, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<List<ReviewResponseDto>>.Error($"获取失败: {ex.Message}");
            }
        }
    }
}
