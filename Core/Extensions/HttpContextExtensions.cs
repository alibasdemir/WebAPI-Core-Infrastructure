using Microsoft.AspNetCore.Http;

namespace Core.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the client IP address from HttpContext, supporting proxy and load balancer scenarios
        /// </summary>
        public static string GetClientIpAddress(this HttpContext? httpContext)
        {
            if (httpContext == null) return "Unknown";

            // Check for forwarded IP (behind proxy/load balancer)
            var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                var ips = forwardedFor.Split(',');
                return ips[0].Trim();
            }

            // Check for real IP header
            var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
                return realIp;

            // Fall back to remote IP
            return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }
}
