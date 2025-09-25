using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace ECommerce.Api.Middleware
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred.");

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var response = new
            {
                error = "An unexpected error occurred. Please try again later."
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response), cancellationToken);

            return true;
        }
    }
}
