using AutoMapper;
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
    /// 收货地址服务
    /// </summary>
    public class AddressService : BaseService
    {
        private readonly IBaseRepository<ShopAddress> _repository;
        private readonly IMapper _mapper;
        private readonly ISqlSugarClient _db;
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AddressService(
            IBaseRepository<ShopAddress> repository,
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
        /// 获取用户收货地址列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>地址列表</returns>
        [HttpPost("GetList")]
        public async Task<ResultDto<List<AddressResponseDto>>> GetListAsync([FromBody] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return ResultDto<List<AddressResponseDto>>.BadRequest("用户ID不能为空");
                }

                var list = await _repository.GetListAsync(x => x.UserId == userId);
                var result = _mapper.Map<List<AddressResponseDto>>(list.OrderByDescending(x => x.IsDefault).ThenByDescending(x => x.CreateDate).ToList());
                return ResultDto<List<AddressResponseDto>>.Success(result, "获取成功");
            }
            catch (Exception ex)
            {
                return ResultDto<List<AddressResponseDto>>.Error($"获取失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建收货地址
        /// </summary>
        /// <param name="dto">地址信息</param>
        /// <returns>创建结果</returns>
        [HttpPost("Create")]
        public async Task<ResultDto<bool>> CreateAsync([FromBody] AddressCreateDto dto)
        {
            try
            {
                var userId = dto.UserId ?? _currentUserService.User?.UserCode;
                if (string.IsNullOrEmpty(userId))
                {
                    return ResultDto<bool>.Error("未获取到用户信息", 401);
                }

                // 如果设为默认地址，先清除该用户的其他默认地址
                if (dto.IsDefault == "1")
                {
                    await _db.Updateable<ShopAddress>()
                        .SetColumns(x => x.IsDefault == "0")
                        .Where(x => x.UserId == userId && x.IsDefault == "1")
                        .ExecuteCommandAsync();
                }

                var entity = _mapper.Map<ShopAddress>(dto);
                if (string.IsNullOrEmpty(entity.AddressId))
                {
                    entity.AddressId = Guid.NewGuid().ToString("N");
                }
                entity.UserId = userId;
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
        /// 更新收货地址
        /// </summary>
        /// <param name="dto">地址信息</param>
        /// <returns>更新结果</returns>
        [HttpPost("Update")]
        public async Task<ResultDto<bool>> UpdateAsync([FromBody] AddressCreateDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.AddressId))
                {
                    return ResultDto<bool>.BadRequest("地址ID不能为空");
                }

                var entity = await _repository.GetByIdAsync(dto.AddressId);
                if (entity == null)
                {
                    return ResultDto<bool>.Error("地址不存在");
                }

                // 如果设为默认地址，先清除该用户的其他默认地址
                if (dto.IsDefault == "1")
                {
                    await _db.Updateable<ShopAddress>()
                        .SetColumns(x => x.IsDefault == "0")
                        .Where(x => x.UserId == entity.UserId && x.IsDefault == "1" && x.AddressId != dto.AddressId)
                        .ExecuteCommandAsync();
                }

                entity.ReceiverName = dto.ReceiverName;
                entity.ReceiverPhone = dto.ReceiverPhone;
                entity.Province = dto.Province;
                entity.City = dto.City;
                entity.District = dto.District;
                entity.DetailAddress = dto.DetailAddress;
                entity.IsDefault = dto.IsDefault;
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
        /// 删除收货地址
        /// </summary>
        /// <param name="addressId">地址ID</param>
        /// <returns>删除结果</returns>
        [HttpPost("Delete")]
        public async Task<ResultDto<bool>> DeleteAsync([FromBody] string addressId)
        {
            try
            {
                if (string.IsNullOrEmpty(addressId))
                {
                    return ResultDto<bool>.BadRequest("地址ID不能为空");
                }

                var result = await _repository.DeleteByIdAsync(addressId);
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
        /// 设置默认地址
        /// </summary>
        /// <param name="addressId">地址ID</param>
        /// <returns>设置结果</returns>
        [HttpPost("SetDefault")]
        public async Task<ResultDto<bool>> SetDefaultAsync([FromBody] string addressId)
        {
            try
            {
                if (string.IsNullOrEmpty(addressId))
                {
                    return ResultDto<bool>.BadRequest("地址ID不能为空");
                }

                var entity = await _repository.GetByIdAsync(addressId);
                if (entity == null)
                {
                    return ResultDto<bool>.Error("地址不存在");
                }

                // 清除该用户所有默认地址
                await _db.Updateable<ShopAddress>()
                    .SetColumns(x => x.IsDefault == "0")
                    .Where(x => x.UserId == entity.UserId && x.IsDefault == "1")
                    .ExecuteCommandAsync();

                // 设置当前地址为默认
                entity.IsDefault = "1";
                entity.UpdateDate = DateTime.Now;

                var result = await _repository.UpdateAsync(entity);
                return result > 0
                    ? ResultDto<bool>.Success(true, "设置成功")
                    : ResultDto<bool>.Error("设置失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"设置失败: {ex.Message}");
            }
        }
    }
}
