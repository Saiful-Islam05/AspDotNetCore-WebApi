using System.Net;
using System.Text.Json;

namespace StudentAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // ✅ পরের middleware চালাও
                await _next(context);
            }
            catch (Exception ex)
            {
                // ❌ Error হলে এখানে আসবে
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            // Error type দেখে Status Code ঠিক করো
            var statusCode = ex switch
            {
                KeyNotFoundException => 404,
                UnauthorizedAccessException => 401,
                ArgumentException => 400,
                _ => 500  // Default
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = ex.Message,
                Type = ex.GetType().Name
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}