using qvzhongren.Model;
using qvzhongren.Shared.Common;
using SqlSugar;
using System.Linq.Expressions;

namespace qvzhongren.Repository.SqlSugar
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        ISqlSugarClient Db { get; }
        ICurrentUserService CurrentUserService { get; }

        #region 事务

        void BeginTran();

        void CommitTran();

        void RollbackTran();

        void Dispose();
        #endregion

        Task<int> GetSequenceValueAsync(string sequenceName);

        #region 查询方法
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// 获取所有实体
        /// </summary>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 获取动态查询对象
        /// </summary>
        ISugarQueryable<TEntity> AsQueryable();

        /// <summary>
        /// 执行自定义SQL查询
        /// </summary>
        Task<List<TResult>> SqlQueryAsync<TResult>(string sql, object parameters = null);

        /// <summary>
        /// 执行自定义SQL查询并返回动态结果
        /// </summary>
        Task<dynamic> SqlQueryDynamicAsync(string sql, object parameters = null);

        /// <summary>
        /// 多表联合查询
        /// </summary>
        Task<List<TResult>> QueryJoinAsync<T1, T2, TResult>(
            Expression<Func<T1, T2, bool>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereExpression = null) where T1 : class, new() where T2 : class, new();

        /// <summary>
        /// 多表联合查询(三表)
        /// </summary>
        Task<List<TResult>> QueryJoinAsync<T1, T2, T3, TResult>(
            Expression<Func<T1, T2, T3, bool>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expression<Func<T1, T2, T3, bool>> whereExpression = null)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new();

        /// <summary>
        /// 分页查询
        /// </summary>
        Task<(List<TEntity> Items, int Total)> GetPageListAsync(Expression<Func<TEntity, bool>> expression, int index = 1, int size = 10);
        #endregion


        /// <summary>
        /// 插入或更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> InsertOrUpdateAsync(TEntity entity);

        /// <summary>
        /// 批量插入或更新实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> InsertOrUpdateRangeAsync(List<TEntity> entities);


        Task<bool> GridSave(List<TEntity> oldEntities, List<TEntity> newEntities);

        #region 插入方法
        /// <summary>
        /// 插入单个实体
        /// </summary>
        Task<int> InsertAsync(TEntity entity);

        /// <summary>
        /// 批量插入实体
        /// </summary>
        Task<int> InsertAsync(List<TEntity> entities);
        #endregion

        #region 更新方法
        /// <summary>
        /// 更新单个实体
        /// </summary>
        Task<int> UpdateAsync(TEntity entity);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        Task<int> UpdateAsync(List<TEntity> entities);

        /// <summary>
        /// 批量更新相同值
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 更新实体限制列
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(TEntity entity, Action<TEntity> columns);

        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateWithOptLockAsync(TEntity entity);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateRangeWithOptLockAsync(List<TEntity> entities);

        #endregion

        #region 删除方法
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        Task<int> DeleteByIdAsync(object id);

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 删除实体
        /// </summary>
        Task<int> DeleteAsync(TEntity entity);
        #endregion
    }
}
