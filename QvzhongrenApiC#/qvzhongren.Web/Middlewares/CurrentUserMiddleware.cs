using qvzhongren.Shared.Common;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using qvzhongren.Application.Common;

namespace qvzhongren.Web.Middlewares
{
    /// <summary>
    /// 当前用户中间件 - 自动从JWT token中提取用户信息
    /// </summary>
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, CurrentUserService currentUserService)
        {
            // 从HttpContext中初始化当前用户
            currentUserService.InitializeFromHttpContext(context);

            // 调用管道中的下一个中间件
            await _next(context);
        }
    }

    /// <summary>
    /// 当前用户中间件扩展方法
    /// </summary>
    public static class CurrentUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CurrentUserMiddleware>();
        }
    }
} 