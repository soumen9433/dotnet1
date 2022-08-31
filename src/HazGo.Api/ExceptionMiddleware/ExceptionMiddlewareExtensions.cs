namespace HazGo.Api.ExceptionMiddleware
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using HazGo.BuildingBlocks.Core.Common;
    using HazGo.Api.Exception;
    using HazGo.Application.Common.Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;

    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app, bool isProduction)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    await WriteResponse(context, isProduction);
                });
            });
        }

        private static async Task WriteResponse(HttpContext httpContext, bool isProduction)
        {
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var error = exceptionDetails?.Error;
            var statusCode = StatusCodes.Status400BadRequest;
            string details = null;
            if (error != null)
            {
                httpContext.Response.ContentType = "application/problem+json";
                var errors = error switch
                {
                    ValidationException ve => ve.Errors.Select(x => new ValidationError(x.Key, string.Join(",", x.Value))).ToList(),
                    _ => null
                };

                if (errors == null)
                {
                    statusCode = StatusCodes.Status500InternalServerError;
                    var errorMessage = isProduction ? "Something went wrong" : "An error occured: " + error.Message;

                    errors = new List<ValidationError>
                    {
                        new ValidationError(string.Empty, errorMessage)
                    };
                    details = isProduction ? null : error.ToString();
                }

                var errorResponse = PrepareErrorResponse(details, errors, statusCode);

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                httpContext.Response.StatusCode = statusCode;
                await JsonSerializer.SerializeAsync(httpContext.Response.Body, errorResponse, jsonOptions);
            }
        }

        private static ErrorResponse PrepareErrorResponse(string details, IEnumerable<ValidationError> errors, int statusCode)
        {
            var errorResponse = new ErrorResponse
            {
                Status = statusCode,
                Title = "Error",
                Detail = details,
                Type = "ValidatonError",
                Errors = errors
            };

            return errorResponse;
        }
    }
}
