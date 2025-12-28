using System.Net;

namespace WebApi.Middleware
{
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenAuthenticationMiddleware> _logger;
        private readonly string _apiKey;

        public TokenAuthenticationMiddleware(RequestDelegate next, ILogger<TokenAuthenticationMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            // Look for environment override or configuration key "ApiKey"
            _apiKey = configuration["ApiKey"] ?? "dev-token";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Allow unauthenticated access to Swagger and static content
            var path = context.Request.Path.Value ?? string.Empty;
            if (path.StartsWith("/swagger") || path == "/")
            {
                await _next(context);
                return;
            }

            // Expect header: Authorization: Bearer <token>
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Missing Authorization header." });
                return;
            }

            var header = authHeader.ToString();
            if (!header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid Authorization header format." });
                return;
            }

            var token = header["Bearer ".Length..].Trim();
            if (string.IsNullOrEmpty(token) || token != _apiKey)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid token." });
                return;
            }

            // token valid; proceed
            await _next(context);
        }
    }
}