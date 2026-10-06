using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Common;

namespace DesignABear.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errorCode = "InternalServerError";
            var message = "An error occurred while processing your request.";

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    errorCode = "NotFound";
                    message = notFoundException.Message;
                    break;
                case BadRequestException badRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorCode = "BadRequest";
                    message = badRequestException.Message;
                    break;
                default:
                    message = exception.Message; 
                    break;
            }

            _logger.LogError(exception, $"[DEBUG] Exception in Middleware: {exception.GetType().Name} - {exception.Message}\nStack Trace: {exception.StackTrace}");

            context.Response.StatusCode = statusCode;
            
            // Format consistently with the system's Result<T>
            // Use 'Description' (capitalized) because that's how it is defined in Error.cs
            var result = Result.Failure(new Error(errorCode, message));
            
            var options = new JsonSerializerOptions { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(result, options);
            
            await context.Response.WriteAsync(json);
        }
    }
}
