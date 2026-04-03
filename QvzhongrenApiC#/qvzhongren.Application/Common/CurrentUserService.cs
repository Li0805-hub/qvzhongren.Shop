using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading;
using qvzhongren.Shared.Common;

namespace qvzhongren.Application.Common
{
    /// <summary>
    /// 当前用户服务实现
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private static AsyncLocal<CurrentUser> _currentUser = new AsyncLocal<CurrentUser>();

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        public CurrentUser User => _currentUser.Value;


        /// <summary>
        /// 设置当前用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        public void SetCurrentUser(CurrentUser user)
        {
            _currentUser.Value = user;
        }

        /// <summary>
        /// 从 HttpContext 初始化当前用户信息
        /// </summary>
        /// <param name="httpContext">HTTP上下文</param>
        public void InitializeFromHttpContext(HttpContext httpContext)
        {
            //if (httpContext?.User?.Identity?.IsAuthenticated != true)
            //{
            //    _currentUser.Value = null;
            //    return;
            //}

            var currentUser = new CurrentUser
            {
                UserCode = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                           httpContext.User.FindFirstValue("sub"),
                UserName = httpContext.User.FindFirstValue(ClaimTypes.Name) ??
                           httpContext.User.FindFirstValue("name"),
                LoginTime = System.DateTime.Now
            };

            // 从Claims中获取其他信息
            foreach (var claim in httpContext.User.Claims)
            {
                switch (claim.Type)
                {
                    case "deptCode":
                        currentUser.DeptCode = claim.Value;
                        break;
                    case "deptName":
                        currentUser.DeptName = claim.Value;
                        break;
                    case "emplType":
                        currentUser.UserType = claim.Value;
                        break;
                    case "hosUnitCode":
                        currentUser.HosUnitCode = claim.Value ?? "Root";
                        break;
                    case "roleCode":
                        currentUser.RoleCode = claim.Value;
                        break;
                    default:
                        // 其他自定义信息存入AdditionalData
                        if (!currentUser.AdditionalData.ContainsKey(claim.Type))
                        {
                            currentUser.AdditionalData[claim.Type] = claim.Value;
                        }
                        break;
                }
            }

            // 保存token
            if (httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var token = authHeader.ToString();
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    currentUser.Token = token.Substring("Bearer ".Length).Trim();
                }
            }
            currentUser.UserCode = currentUser.UserCode ?? "009999";
            currentUser.HosUnitCode = currentUser.HosUnitCode ?? "Root";
            _currentUser.Value = currentUser;
        }

        /// <summary>
        /// 清除当前用户信息
        /// </summary>
        public void ClearCurrentUser()
        {
            _currentUser.Value = null;
        }


    }
}