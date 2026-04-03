using Castle.DynamicProxy;
using qvzhongren.Application.Logging;

namespace qvzhongren.Application.Interceptors
{
    /// <summary>
    /// 接口调用日志
    /// </summary>
    public class LoggingInterceptor : IInterceptor
    {
        private readonly Logger _logger;

        public LoggingInterceptor(Logger logger)
        {
            _logger = logger;
        }

        public async void Intercept(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.DeclaringType?.FullName}.{invocation.Method.Name}-{Guid.NewGuid()}";
            try
            {
                // 接口调用前
                var startTime = DateTime.Now;
                // 日志记录走另外进程
                _ = Task.Run(() => _logger.LogInformationAsync(methodName, $"开始调用，参数为: {Newtonsoft.Json.JsonConvert.SerializeObject(invocation.Arguments)}"));
                
                // 执行接口走主进程
                invocation.Proceed();
                
                // 接口调用结束
                var endTime = DateTime.Now;
                var duration = (endTime - startTime).TotalSeconds;

                // 耗时警告日志走另外进程
                if (duration > 3)
                {
                    _ = Task.Run(() => _logger.LogWarningAsync(methodName, $"接口执行时间过长: {duration:F2}秒"));
                }
                
                // 返回值日志走另外进程
                _ = Task.Run(() => _logger.LogInformationAsync(methodName, $"执行结束，返回值为: {Newtonsoft.Json.JsonConvert.SerializeObject(invocation.ReturnValue)}"));
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(methodName, $"接口执行错误.错误信息：{ex.Message}");
                throw;
            }
        }
    }
}