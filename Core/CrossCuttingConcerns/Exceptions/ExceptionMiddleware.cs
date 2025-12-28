using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Core.CrossCuttingConcerns.Exceptions
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
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Internal Server Error";
            var errors = new List<string>();

            if (exception is ValidationException validationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = "Validation Error";
                errors = validationException.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = (int)statusCode,
                Message = message,
                Errors = errors.Any() ? errors : new List<string> { exception.Message }
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
