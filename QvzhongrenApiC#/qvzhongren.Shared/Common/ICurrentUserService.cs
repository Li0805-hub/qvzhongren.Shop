namespace qvzhongren.Shared.Common
{
    /// <summary>
    /// 当前用户服务接口
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        CurrentUser User { get; }

        /// <summary>
        /// 设置当前用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        void SetCurrentUser(CurrentUser user);

        /// <summary>
        /// 清除当前用户信息
        /// </summary>
        void ClearCurrentUser();
    }
} 