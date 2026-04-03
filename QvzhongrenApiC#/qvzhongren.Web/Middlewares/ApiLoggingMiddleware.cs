using qvzhongren.Application.Logging;

namespace qvzhongren.Web.Middlewares;

public class ApiLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ApiLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? "";
        if (!path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase)
            || path.Contains("/Log/", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var method = context.Request.Method;
        var name = $"{method} {path}";
        var startTime = DateTime.Now;

        // 1. Read request body
        string? requestBody = null;
        context.Request.EnableBuffering();
        if (context.Request.ContentLength is > 0)
        {
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        // 2. Replace response body stream to capture output
        var originalBody = context.Response.Body;
        using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        string? responseBody = null;
        try
        {
            await _next(context);

            // 3. Read response body
            memStream.Position = 0;
            responseBody = await new StreamReader(memStream).ReadToEndAsync();

            // 4. Copy back to original stream
            memStream.Position = 0;
            await memStream.CopyToAsync(originalBody);
        }
        catch (Exception ex)
        {
            context.Response.Body = originalBody;
            var logger = context.RequestServices.GetService<Logger>();
            if (logger != null)
            {
                _ = Task.Run(() => logger.LogAsync(name, "Error", $"异常: {ex.Message}", requestBody, null));
            }
            throw;
        }
        finally
        {
            context.Response.Body = originalBody;
        }

        // 5. Write log
        var duration = (DateTime.Now - startTime).TotalMilliseconds;
        var statusCode = context.Response.StatusCode;
        var type = statusCode >= 400 ? "Error" : (duration > 3000 ? "Warning" : "Information");
        var content = $"状态码: {statusCode}, 耗时: {duration:F0}ms";

        var log = context.RequestServices.GetService<Logger>();
        if (log != null)
        {
            _ = Task.Run(() => log.LogAsync(name, type, content, requestBody, responseBody));
        }
    }
}
