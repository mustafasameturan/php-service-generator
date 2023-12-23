using Microsoft.Extensions.Primitives;

namespace KoGenerator.Middlewares;

public class ApiKeyAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyAuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var requestPath = httpContext.Request.Path.Value ?? "/";

        if (!httpContext.Request.Headers.Contains(new KeyValuePair<string, StringValues>("api-key", _configuration["Api_Key"]))
            && !requestPath.Contains("swagger"))
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("What are you doing????");
            return;
        }

        await _next(httpContext);
    }
}