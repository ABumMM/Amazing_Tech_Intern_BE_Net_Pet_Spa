using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetSpa.Core.Base;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetSpaBE.API.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError($"Error occurred: {ex}");

            if (!context.Response.HasStarted)
            {
                var response = new BaseResponseModel<object>(
                    StatusCodes.Status500InternalServerError,
                    ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                    null,
                    "An unexpected error occurred."
                );

                if (ex is CoreException coreException)
                {
                    response.StatusCode = coreException.StatusCode;
                    response.Code = coreException.Code;
                    response.Message = coreException.Message;

                    // Ghi thêm dữ liệu bổ sung nếu có
                    if (coreException.AdditionalData.Any())
                    {
                        response.AdditionalData = JsonSerializer.Serialize(coreException.AdditionalData);
                    }
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = response.StatusCode;

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}