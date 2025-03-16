using System.Text.Json;
using TelephoneSwitch.Server.CustomExceptions;

namespace TelephoneSwitch.Server.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await HandleExceptionAsync(context, "Unauthorized", StatusCodes.Status401Unauthorized);
            }
            catch (ForbiddenAccessException)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await HandleExceptionAsync(context, "Forbidden", StatusCodes.Status403Forbidden);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, "Internal Server Error", StatusCodes.Status500InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string message, int statusCode)
        {
            var response = new { error = message };
            var json = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(json);
        }
    }
}
