using System.Net;
using Application.Common;

namespace Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                new { success = false, message = exception.Message }
            );
        }
        catch (UnauthorizedAccessException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                new { success = false, message = exception.Message }
            );
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                new { success = false, message = "Internal server error." }
            );
        }
    }
}
