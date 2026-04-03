using qvzhongren.Model;
using qvzhongren.Shared.Common;
using SqlSugar;
using System.Linq.Expressions;
using System.Reflection;

namespace qvzhongren.Repository.SqlSugar
{
    /// <summary>
    /// 基础仓储实现类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly ISqlSugarClient _db;

        /// <summary>
        /// 仅用于测试，上线后删除
        /// </summary>
        public ISqlSugarClient Db => _db;

        /// <summary>
        /// 当前登录人信息
        /// </summary>
        protected readonly ICurrentUserService _currentUserService;

        public ICurrentUserService CurrentUserService => _currentUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="currentUserService">当前用户服务</param>
        public BaseRepository(SqlSugarDbContext dbContext, ICurrentUserService currentUserService)
        {
            _db = dbContext.Db;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// 设置审计信息，存在对应字段时才赋值
        /// </summary>
        /// <param name="entity">要设置的实体</param>
        /// <param name="isCreate">是否是创建操作</param>
        private void SetAuditInfo(TEntity entity, bool isCreate = false)
        {
            string userCode = _currentUserService.User.UserCode ?? "009999";
            DateTime now = DateTime.Now;

            // 检查并设置医院编码
            entity.HosUnitCode = string.IsNullOrEmpty(entity.HosUnitCode) ? "Root" : entity.HosUnitCode;

            // 获取实体类型
            Type entityType = entity.GetType();

            // 检查并设置更新信息
            PropertyInfo updateCodeProp = entityType.GetProperty("UpdateCode");
            if (updateCodeProp != null && updateCodeProp.CanWrite)
            {
                updateCodeProp.SetValue(entity, userCode);
            }

            PropertyInfo updateDateProp = entityType.GetProperty("UpdateDate");
            if (updateDateProp != null && updateDateProp.CanWrite)
            {
                updateDateProp.SetValue(entity, now);
            }

            // 如果是创建操作，检查并设置创建信息
            if (isCreate)
            {
                PropertyInfo createCodeProp = entityType.GetProperty("CreateCode");
                if (createCodeProp != null && createCodeProp.CanWrite)
                {
                    createCodeProp.SetValue(entity, userCode);
                }

                PropertyInfo createDateProp = entityType.GetProperty("CreateDate");
                if (createDateProp != null && createDateProp.CanWrite)
                {
                    createDateProp.SetValue(entity, now);
                }
            }
        }

        /// <summary>
        /// 获取序列值
        /// </summary>
        /// <param name="sequenceName">序列名称</param>
        /// <returns>序列值</returns>
        public virtual async Task<int> GetSequenceValueAsync(string sequenceName)
        {
            return await _db.Ado.GetIntAsync($"SELECT nextval('{sequenceName}')");
        }

        #region 事务

        public virtual void BeginTran()
        {
            _db.Ado.BeginTran();
        }

        public virtual void CommitTran()
        {
            _db.Ado.CommitTran();
        }

        public virtual void RollbackTran()
        {
            _db.Ado.RollbackTran();
        }

        public virtual void Dispose()
        {
            _db?.Dispose();
        }
        #endregion

        #region 查询方法
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>实体对象</returns>
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _db.Queryable<TEntity>().InSingleAsync(id);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="expression">查询条件表达式</param>
        /// <returns>实体对象</returns>
        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _db.Queryable<TEntity>().FirstAsync(expression);
        }

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <param name="expression">查询条件表达式</param>
        /// <returns>实体列表</returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _db.Queryable<TEntity>().Where(expression).ToListAsync();
        }

        /// <summary>
        /// 获取动态查询对象,用于复杂查询
        /// </summary>
        /// <returns>ISugarQueryable对象</returns>
        public virtual ISugarQueryable<TEntity> AsQueryable()
        {
            return _db.Queryable<TEntity>();
        }

        /// <summary>
        /// 执行自定义SQL查询
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>查询结果</returns>
        public virtual async Task<List<TResult>> SqlQueryAsync<TResult>(string sql, object parameters = null)
        {
            return await _db.Ado.SqlQueryAsync<TResult>(sql, parameters);
        }

        /// <summary>
        /// 多表联合查询
        /// </summary>
        /// <typeparam name="T1">第一个表</typeparam>
        /// <typeparam name="T2">第二个表</typeparam>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="joinExpression">关联条件</param>
        /// <param name="selectExpression">选择表达式</param>
        /// <param name="whereExpression">查询条件</param>
        /// <returns>查询结果</returns>
        public virtual async Task<List<TResult>> QueryJoinAsync<T1, T2, TResult>(
            Expression<Func<T1, T2, bool>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereExpression = null) where T1 : class, new() where T2 : class, new()
        {
            var query = _db.Queryable<T1, T2>(joinExpression);
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }

            return await query.Select(selectExpression).ToListAsync();
        }

        /// <summary>
        /// 多表联合查询(三表)
        /// </summary>
        /// <typeparam name="T1">第一个表</typeparam>
        /// <typeparam name="T2">第二个表</typeparam>
        /// <typeparam name="T3">第三个表</typeparam>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="joinExpression">第一个关联条件</param>
        /// <param name="selectExpression">第二个关联条件</param>
        /// <param name="whereExpression">查询条件</param>
        /// <returns>查询结果</returns>
        public virtual async Task<List<TResult>> QueryJoinAsync<T1, T2, T3, TResult>(
            Expression<Func<T1, T2, T3, bool>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expression<Func<T1, T2, T3, bool>> whereExpression = null)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            var query = _db.Queryable(joinExpression);

            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }
            return await query.Select(selectExpression).ToListAsync();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="expression">查询条件表达式</param>
        /// <param name="pageDto">分页参数</param>
        /// <returns>分页结果</returns>
        public virtual async Task<(List<TEntity> Items, int Total)> GetPageListAsync(Expression<Func<TEntity, bool>> expression, int index = 1, int size = 10)
        {

            // 参数验证和规范化
            index = Math.Max(1, index);
            size = Math.Max(10, Math.Min(100, size)); // 限制每页最大数量为100

            // 创建查询对象,避免重复创建
            var query = _db.Queryable<TEntity>().Where(expression);

            // 并行获取总数和分页数据
            var totalTask = query.CountAsync();
            var dataTask = query.Skip((index - 1) * size)
                               .Take(size)
                               .ToListAsync();

            await Task.WhenAll(totalTask, dataTask);

            return (await dataTask, await totalTask);
        }
        #endregion


        #region 插入或更新

        /// <summary>
        ///  插入或更新单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> InsertOrUpdateAsync(TEntity entity)
        {
            var x = _db.Storageable(entity).ToStorage();
            if (x.InsertList.Any())
            {
                foreach (var item in x.InsertList)
                {
                    SetAuditInfo(item.Item, true);
                }
            }
            if (x.UpdateList.Any())
            {
                foreach (var item in x.UpdateList)
                {
                    SetAuditInfo(item.Item);
                }
            }
            int insertCount = await x.AsInsertable.ExecuteCommandAsync();
            int updateCount = await x.AsUpdateable.IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            if (insertCount > 0 || updateCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 插入或更新多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<bool> InsertOrUpdateRangeAsync(List<TEntity> entities)
        {

            BeginTran();
            var x = _db.Storageable(entities).ToStorage();
            if (x.InsertList.Any())
            {
                foreach (var item in x.InsertList)
                {
                    SetAuditInfo(item.Item, true);
                }
            }
            if (x.UpdateList.Any())
            {
                foreach (var item in x.UpdateList)
                {
                    SetAuditInfo(item.Item);
                }
            }
            int insertCount = await x.AsInsertable.ExecuteCommandAsync();
            int updateCount = await x.AsUpdateable.ExecuteCommandAsync();
            if (insertCount + updateCount == entities.Count)
            {
                CommitTran();
                return true;
            }
            else
            {
                RollbackTran();
                return false;
            }
        }

        /// <summary>
        /// 比较两个集合，实现批量插入、更新、删除
        /// </summary>
        /// <param name="oldEntities"></param>
        /// <param name="newEntities"></param>
        /// <returns></returns>
        public virtual async Task<bool> GridSave(List<TEntity> oldEntities, List<TEntity> newEntities)
        {
            foreach (var entity in newEntities.Except(oldEntities))
            {
                SetAuditInfo(entity, true);
            }
            foreach (var entity in newEntities.Intersect(oldEntities))
            {
                SetAuditInfo(entity);
            }
            return await _db.GridSave(oldEntities, newEntities).ExecuteCommandAsync();
        }
        #endregion    

        #region 插入方法
        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            SetAuditInfo(entity, true);
            return await _db.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>影响行数</returns>

        public virtual async Task<int> InsertAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetAuditInfo(entity, true);
            }
            return await _db.Insertable(entities).ExecuteCommandAsync();
        }
        #endregion

        #region 更新方法
        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            SetAuditInfo(entity);
            return await _db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> UpdateWithOptLockAsync(TEntity entity)
        {
            SetAuditInfo(entity);
            return await _db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandWithOptLockAsync(true);
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> UpdateRangeWithOptLockAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetAuditInfo(entity);
            }
            return await _db.Updateable(entities).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandWithOptLockAsync(true);
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> UpdateAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetAuditInfo(entity);
            }
            return await _db.Updateable(entities).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="columns">要更新的列</param>
        /// <param name="expression">查询条件</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> expression)
        {

            BeginTran();
            // 创建更新对象
            var updateable = await _db.Updateable<TEntity>()                 
                   .SetColumns(columns)
                   .SetColumns(x => new TEntity { HosUnitCode = (string.IsNullOrEmpty(_currentUserService.User.HosUnitCode) ? "Root" : _currentUserService.User.HosUnitCode) })
                   .Where(expression).ExecuteCommandAsync();

            bool isBaseUpdateInfoEntity = typeof(BaseUpdateInfoEntity).IsAssignableFrom(typeof(TEntity));
            if (updateable > 0 && isBaseUpdateInfoEntity)
            {
                var list = await _db.Queryable<TEntity>().Where(expression).ToListAsync();

                foreach (var entity in list)
                {
                    Type entityType = entity.GetType();

                    PropertyInfo updateCodeProp = entityType.GetProperty("UpdateCode");
                    if (updateCodeProp != null && updateCodeProp.CanWrite)
                    {
                        updateCodeProp.SetValue(entity, _currentUserService.User.UserCode ?? "009999");
                    }

                    PropertyInfo updateDateProp = entityType.GetProperty("UpdateDate");
                    if (updateDateProp != null && updateDateProp.CanWrite)
                    {
                        updateDateProp.SetValue(entity, DateTime.Now);
                    }

                }

                var updateInfo = await _db.Updateable(list).ExecuteCommandAsync();
                if (updateInfo != updateable)
                {
                    RollbackTran();
                    return 0;
                }
            }
            CommitTran();
            return updateable;
        }
        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entity">实体列表</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> UpdateAsync(TEntity entity, Action<TEntity> columns)
        {
            return await _db.Updateable(entity)
                .IgnoreColumns(ignoreAllNullColumns: true)
                .ReSetValue(columns)
                .ReSetValue(x =>
                {
                    Type entityType = x.GetType();

                    PropertyInfo updateCodeProp = entityType.GetProperty("UpdateCode");
                    if (updateCodeProp != null && updateCodeProp.CanWrite)
                    {
                        updateCodeProp.SetValue(x, _currentUserService.User.UserCode ?? "009999");
                    }

                    PropertyInfo updateDateProp = entityType.GetProperty("UpdateDate");
                    if (updateDateProp != null && updateDateProp.CanWrite)
                    {
                        updateDateProp.SetValue(x, DateTime.Now);
                    }

                    x.HosUnitCode = string.IsNullOrEmpty(x.HosUnitCode) ? "Root" : x.HosUnitCode;
                })
                .ExecuteCommandAsync();
        }
        #endregion

        #region 删除方法
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        public virtual async Task<int> DeleteByIdAsync(object id)
        {
            return await _db.Deleteable<TEntity>().In(id).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _db.Deleteable<TEntity>().Where(expression).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            return await _db.Deleteable(entity).ExecuteCommandAsync();
        }

        public Task<dynamic> SqlQueryDynamicAsync(string sql, object parameters = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
