using Core.Dynamic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Core.Application.Pipelines.Caching
{
    public static class CacheKeyHelper
    {
        /// <summary>
        /// Generates a hash from DynamicQuery for cache key
        /// </summary>
        public static string GetDynamicQueryHash(DynamicQuery? dynamic)
        {
            if (dynamic == null)
                return "NoDynamic";

            var json = JsonSerializer.Serialize(dynamic);
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
            return Convert.ToBase64String(hash)[..8];
        }

        /// <summary>
        /// Generates a hash from any object for cache key
        /// </summary>
        public static string GetObjectHash(object? obj)
        {
            if (obj == null)
                return "NoData";

            var json = JsonSerializer.Serialize(obj);
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
            return Convert.ToBase64String(hash)[..8];
        }
    }
}
