using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace Core.Application.Pipelines.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ILoggableRequest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoggerServiceBase _loggerServiceBase;

        public LoggingBehavior(LoggerServiceBase loggerServiceBase, IHttpContextAccessor httpContextAccessor)
        {
            _loggerServiceBase = loggerServiceBase;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            // Request details
            string requestName = typeof(TRequest).Name;
            string requestFullName = typeof(TRequest).FullName ?? requestName;
            string user = GetCurrentUser();
            string ipAddress = GetClientIpAddress();
            string userAgent = GetUserAgent();

            // Build parameters
            List<LogParameter> logParameters = BuildLogParameters(request);

            // Log request
            LogDetail requestLog = new()
            {
                FullName = requestFullName,
                MethodName = requestName,
                User = user,
                Parameters = logParameters
            };

            _loggerServiceBase.Info($"[REQUEST] {requestName} | User: {user} | IP: {ipAddress} | Data: {JsonSerializer.Serialize(requestLog)}");

            TResponse response;
            try
            {
                // Execute request
                response = await next();
                stopwatch.Stop();

                // Log successful response
                _loggerServiceBase.Info($"[SUCCESS] {requestName} | User: {user} | Duration: {stopwatch.ElapsedMilliseconds}ms");

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                // Log error
                LogDetailWithException errorLog = new()
                {
                    FullName = requestFullName,
                    MethodName = requestName,
                    User = user,
                    Parameters = logParameters,
                    ExceptionMessage = ex.Message
                };

                _loggerServiceBase.Error($"[ERROR] {requestName} | User: {user} | Duration: {stopwatch.ElapsedMilliseconds}ms | Error: {ex.Message} | Details: {JsonSerializer.Serialize(errorLog)}");

                throw;
            }
        }

        private List<LogParameter> BuildLogParameters(TRequest request)
        {
            List<LogParameter> logParameters = new();
            var properties = request.GetType().GetProperties();

            foreach (var property in properties)
            {
                var name = property.Name;
                var value = property.GetValue(request);

                // Mask sensitive data
                if (IsSensitiveProperty(name))
                {
                    value = "***MASKED***";
                }

                logParameters.Add(new LogParameter
                {
                    Name = name,
                    Value = value,
                    Type = property.PropertyType.Name
                });
            }

            return logParameters;
        }

        private bool IsSensitiveProperty(string propertyName)
        {
            string[] sensitiveProperties =
            {
                "Password",
                "PasswordHash",
                "PasswordSalt",
                "Token",
                "SecurityKey",
                "CurrentPassword",
                "NewPassword",
                "PasswordConfirm",
                "NewPasswordConfirm"
            };

            return sensitiveProperties.Contains(propertyName, StringComparer.OrdinalIgnoreCase);
        }

        private string GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return "Anonymous";

            var emailClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var nameClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            return emailClaim ?? nameClaim ?? "Anonymous";
        }

        private string GetClientIpAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return "Unknown";

            var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }

            var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
                return realIp;

            return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        private string GetUserAgent()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return "Unknown";

            return httpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";
        }
    }
}
