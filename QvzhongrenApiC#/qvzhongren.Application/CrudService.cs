namespace qvzhongren.Application
{
    /// <summary>
    /// 通用CRUD服务基类，提供实体的基本增删改查操作
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TUpdateDto">更新DTO类型</typeparam>
    /// <typeparam name="TCreateDto">创建DTO类型</typeparam>
    public abstract class CrudService<TEntity, TUpdateDto, TCreateDto> : BaseService
        where TEntity : BaseAuditEntity, new()
        where TUpdateDto : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper; // 添加映射类的依赖注入

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository">实体仓储</param>
        /// <param name="mapper">AutoMapper映射器</param>
        protected CrudService(IBaseRepository<TEntity> repository, IMapper mapper) // 修改构造函数
        {
            _repository = repository;
            _mapper = mapper; // 初始化映射类
        }

        /// <summary>
        /// 创建单个实体
        /// </summary>
        /// <param name="createDto">创建DTO</param>
        /// <returns>创建结果</returns>
        [HttpPost("Create")]
        public virtual async Task<ResultDto<bool>> CreateAsync([FromBody] TCreateDto createDto)
        {
            try
            {
                var mappedEntity = _mapper.Map<TCreateDto, TEntity>(createDto);
                var result = await _repository.InsertAsync(mappedEntity);
                if (result > 0)
                {

                    // 更新Redis缓存
                    await UpdateRedisAsync(mappedEntity);

                    return ResultDto<bool>.Success(true, $"{typeof(TEntity).Name}新增成功");
                }
                else
                {
                    return ResultDto<bool>.Error($"{typeof(TEntity).Name}新增失败");
                }
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"新增失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量创建实体
        /// </summary>
        /// <param name="createDtos">创建DTO列表</param>
        /// <returns>批量创建结果</returns>
        [HttpPost("CreateRange")]
        public virtual async Task<ResultDto<bool>> CreateAsync([FromBody] List<TCreateDto> createDtos)
        {
            try
            {
                var mappedEntity = _mapper.Map<List<TCreateDto>, List<TEntity>>(createDtos);
                var result = await _repository.InsertAsync(mappedEntity);
                if (result > 0)
                {

                    // 批量更新Redis缓存
                    await UpdateRedisAsync(mappedEntity);

                }
                return result > 0
                    ? ResultDto<bool>.Success(true, $"{typeof(TEntity).Name}批量新增成功")
                    : ResultDto<bool>.Error($"{typeof(TEntity).Name}批量新增失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"批量新增失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>删除结果</returns>
        [HttpPost("Delete")]
        public virtual async Task<ResultDto<bool>> DeleteAsync([FromBody] object id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return ResultDto<bool>.Error("实体不存在");
                }
                var result = await _repository.DeleteAsync(entity);
                if (result > 0)
                {
                    // 从Redis缓存中删除实体
                    await DeleteRedisAsync(entity);
                    return ResultDto<bool>.Success(true, $"{typeof(TEntity).Name}删除成功");
                }
                else
                {
                    return ResultDto<bool>.Error($"{typeof(TEntity).Name}删除失败");
                }
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"删除失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <param name="updateDto">更新DTO</param>
        /// <returns>更新结果</returns>
        [HttpPost("Update")]
        public virtual async Task<ResultDto<bool>> UpdateAsync([FromBody] TUpdateDto updateDto)
        {
            try
            {
                // 使用正常映射更新数据到实体
                var mappedEntity = _mapper.Map<TUpdateDto, TEntity>(updateDto); // 使用依赖注入的映射类

                var result = await _repository.UpdateAsync(mappedEntity);

                // 更新Redis缓存
                await UpdateRedisAsync(mappedEntity);

                return result > 0
                    ? ResultDto<bool>.Success(true, $"{typeof(TEntity).Name}更新成功")
                    : ResultDto<bool>.Error($"{typeof(TEntity).Name}更新失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"更新失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="updateDtos">更新DTO列表</param>
        /// <returns>批量更新结果</returns>
        [HttpPost("UpdateRange")]
        public virtual async Task<ResultDto<bool>> UpdateAsync([FromBody] List<TUpdateDto> updateDtos)
        {
            try
            {
                var mappedEntity = _mapper.Map<List<TUpdateDto>, List<TEntity>>(updateDtos);
                var result = await _repository.UpdateAsync(mappedEntity);
                if (result == updateDtos.Count)
                {

                    // 批量更新Redis缓存
                    await UpdateRedisAsync(mappedEntity);

                }
                return result == updateDtos.Count
                    ? ResultDto<bool>.Success(true, $"批量更新{typeof(TEntity).Name}{result}条数据成功")
                    : ResultDto<bool>.Error($"{typeof(TEntity).Name}批量更新失败");
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"{typeof(TEntity).Name}批量更新失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体对象</returns>
        [HttpPost("GetById")]
        public virtual async Task<ResultDto<TEntity>> GetByIdAsync([FromBody] object id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                return ResultDto<TEntity>.Success(result);
            }
            catch (Exception ex)
            {
                return ResultDto<TEntity>.Error(ex.Message);
            }
        }

        /// <summary>
        /// 刷新Redis缓存
        /// </summary>
        /// <returns>刷新结果</returns>
        [HttpPost("RefreshRedis")]
        public virtual async Task<ResultDto<bool>> RefreshRedisAsync()
        {
            try
            {
                // 从数据库获取所有数据
                var allEntities = await _repository.GetAllAsync();
             
                var result = await RedisHelper.SetAsync(typeof(TEntity).Name, allEntities);

                if (result)
                {
                    return ResultDto<bool>.Success(true, $"已刷新{allEntities.Count}条数据到Redis缓存");
                }
                else
                {
                    return ResultDto<bool>.Error("刷新Redis缓存失败");
                }
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"刷新Redis缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 保存实体（新增或更新）
        /// </summary>
        /// <param name="dto">实体DTO</param>
        /// <returns>保存结果</returns>
        [HttpPost("Save")]
        public virtual async Task<ResultDto<bool>> SaveAsync([FromBody] TUpdateDto dto)
        {
            try
            {
                // 使用正常映射更新数据到实体
                var mappedEntity = _mapper.Map<TUpdateDto, TEntity>(dto);

                // 获取实体类型
                var entityType = typeof(TEntity);

                // 使用InsertOrUpdateAsync进行保存
                var result = await _repository.InsertOrUpdateAsync(mappedEntity);
                if (result)
                {
                    // 更新Redis缓存
                    await UpdateRedisAsync(mappedEntity);
                    return ResultDto<bool>.Success(true, $"{typeof(TEntity).Name}保存成功");
                }
                else
                {
                    return ResultDto<bool>.Error($"{typeof(TEntity).Name}保存失败");
                }
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"保存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量保存实体（新增或更新）
        /// </summary>
        /// <param name="dtos">实体DTO列表</param>
        /// <returns>保存结果</returns>
        [HttpPost("SaveRange")]
        public virtual async Task<ResultDto<bool>> SaveRangeAsync([FromBody] List<TUpdateDto> dtos)
        {
            try
            {
                if (dtos == null || !dtos.Any())
                {
                    return ResultDto<bool>.Error("保存的实体列表不能为空");
                }

                var mappedEntities = _mapper.Map<List<TUpdateDto>, List<TEntity>>(dtos);

                // 使用InsertOrUpdateAsync进行批量保存
                var result = await _repository.InsertOrUpdateRangeAsync(mappedEntities);
                if (result)
                {
                    // 更新Redis缓存
                    await UpdateRedisAsync(mappedEntities);
                    return ResultDto<bool>.Success(true, $"{typeof(TEntity).Name}批量保存成功");
                }
                else
                {
                    return ResultDto<bool>.Error($"{typeof(TEntity).Name}批量保存失败");
                }
            }
            catch (Exception ex)
            {
                return ResultDto<bool>.Error($"批量保存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新Redis中的单个实体数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="key"></param>
        /// <returns>更新结果</returns>
        protected virtual async Task<bool> UpdateRedisAsync(TEntity entity, string key = "")
        {
            // 将单个实体包装成列表，调用批量更新方法，避免代码重复
            return await UpdateRedisAsync(new List<TEntity> { entity }, key);
        }

        /// <summary>
        /// 批量更新Redis中的实体数据
        /// </summary>
        /// <param name="entities">实体对象列表</param>
        /// <param name="key">Redis键名，默认为实体类型名称</param>
        /// <returns>更新结果</returns>
        protected virtual async Task<bool> UpdateRedisAsync(List<TEntity> entities, string key = "")
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = typeof(TEntity).Name;
                }

                if (entities == null || !entities.Any())
                {
                    return true; // 没有实体需要更新
                }

                // 获取实体类型
                var entityType = typeof(TEntity);

                // 获取实体的主键属性，优先使用SugarColumn特性的IsPrimaryKey属性
                var keyProperty = entityType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .FirstOrDefault(p => p.GetCustomAttributes(true).Any(attr =>
                        attr.GetType().Name.Contains("SugarColumn") &&
                        attr.GetType().GetProperty("IsPrimaryKey")?.GetValue(attr) as bool? == true));

                // 如果没有找到SugarColumn主键，尝试查找KeyAttribute
                if (keyProperty == null)
                {
                    return false;
                }

                // 如果仍然找不到主键，尝试常见命名约定


                // 获取当前Redis中的列表，如果不存在则创建新列表
                var currentList = await RedisHelper.GetAsync<List<TEntity>>(key) ?? new List<TEntity>();

                // 遍历要更新的实体
                foreach (var entity in entities)
                {
                    // 获取实体的主键值
                    var id = keyProperty.GetValue(entity);
                    if (id == null)
                    {
                        Console.WriteLine($"[UpdateRedis] 警告：实体 {entityType.Name} 的主键值为空");
                        continue; // 跳过主键为空的实体
                    }

                    // 查找当前列表中是否存在相同主键的实体
                    var existingIndex = currentList.FindIndex(e =>
                    {
                        var eId = keyProperty.GetValue(e);
                        return eId != null && eId.ToString() == id.ToString();
                    });

                    if (existingIndex >= 0)
                    {
                        // 如果存在则更新现有实体
                        currentList[existingIndex] = entity;
                    }
                    else
                    {
                        // 如果不存在则添加新实体
                        currentList.Add(entity);
                    }
                }

                // 将更新后的列表保存回Redis
                await RedisHelper.SetAsync(key, currentList);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateRedis] 异常：{ex.Message}");
                Console.WriteLine($"[UpdateRedis] 堆栈：{ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 从Redis缓存中删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="key">Redis键名，默认为实体类型名称</param>
        /// <returns>操作是否成功</returns>
        protected virtual async Task<bool> DeleteRedisAsync(TEntity entity, string key = "")
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = typeof(TEntity).Name;
                }

                if (entity == null)
                {
                    return false;
                }

                // 获取实体类型
                var entityType = typeof(TEntity);

                // 获取实体的主键属性，优先使用标记有Key特性的属性
                var keyProperty = entityType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .FirstOrDefault(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), true).Any());

                // 如果没有找到标记有Key特性的属性，则尝试常见命名约定（不包括ID）
                if (keyProperty == null)
                {
                    // 输出诊断信息
                    Console.WriteLine($"[DeleteRedis] 尝试查找实体 {entityType.Name} 的主键属性");

                    // 尝试常见主键命名约定（排除ID相关命名）
                    var possibleIdNames = new[] {
                        "Id",
                        entityType.Name + "Id",
                        entityType.Name.ToLower() + "Id",
                        entityType.Name.ToLower() + "_id",
                        "PK_" + entityType.Name,
                        "pk",
                        "primary"
                    };

                    foreach (var idName in possibleIdNames)
                    {
                        keyProperty = entityType.GetProperty(idName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                        if (keyProperty != null)
                        {
                            Console.WriteLine($"[DeleteRedis] 使用 {idName} 作为主键");
                            break;
                        }
                    }

                    // 如果仍然找不到主键，返回失败
                    if (keyProperty == null)
                    {
                        Console.WriteLine($"[DeleteRedis] 警告：无法在 {entityType.Name} 中找到主键属性");
                        return false;
                    }
                }

                // 获取实体的主键值
                var id = keyProperty.GetValue(entity);
                if (id == null)
                {
                    Console.WriteLine($"[DeleteRedis] 警告：实体 {entityType.Name} 的主键值为空");
                    return false;
                }

                // 获取当前Redis中的列表
                var currentList = await RedisHelper.GetAsync<List<TEntity>>(key);
                if (currentList == null || !currentList.Any())
                {
                    return true; // 列表为空，无需删除
                }

                // 从列表中移除实体
                int initialCount = currentList.Count;
                currentList.RemoveAll(e =>
                {
                    var eId = keyProperty.GetValue(e);
                    return eId != null && eId.ToString() == id.ToString();
                });

                // 如果列表发生变化，更新Redis
                if (currentList.Count < initialCount)
                {
                    await RedisHelper.SetAsync(key, currentList);
                    Console.WriteLine($"[DeleteRedis] 从Redis中删除了实体 {entityType.Name} (ID: {id})");
                }
                else
                {
                    Console.WriteLine($"[DeleteRedis] 在Redis中未找到要删除的实体 {entityType.Name} (ID: {id})");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeleteRedis] 异常：{ex.Message}");
                Console.WriteLine($"[DeleteRedis] 堆栈：{ex.StackTrace}");
                return false;
            }
        }
    }
}