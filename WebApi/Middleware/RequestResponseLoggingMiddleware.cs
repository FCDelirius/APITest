using System.Diagnostics;

namespace WebApi.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var method = context.Request.Method;
            var path = context.Request.Path;

            await _next(context);

            sw.Stop();
            var status = context.Response.StatusCode;

            _logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms", method, path, status, sw.ElapsedMilliseconds);
        }
    }
}