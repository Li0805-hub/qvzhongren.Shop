using qvzhongren.Contracts.Clients;

namespace qvzhongren.Application.Logging
{
    /// <summary>
    /// Logger 委托给 ILogWriter 实现，不直接依赖任何 Model
    /// </summary>
    public class Logger
    {
        private readonly ILogWriter _writer;

        public Logger(ILogWriter writer)
        {
            _writer = writer;
        }

        public async Task LogInformationAsync(string name, string content)
        {
            await _writer.LogInformationAsync(name, content);
        }

        public async Task LogWarningAsync(string name, string content)
        {
            await _writer.LogWarningAsync(name, content);
        }

        public async Task LogErrorAsync(string name, string content)
        {
            await _writer.LogErrorAsync(name, content);
        }

        public async Task LogAsync(string name, string type, string content, string? requestBody = null, string? responseBody = null)
        {
            await _writer.LogAsync(name, type, content, requestBody, responseBody);
        }
    }
}
