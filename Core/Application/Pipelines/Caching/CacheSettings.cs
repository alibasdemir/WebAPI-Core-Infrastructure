namespace Core.Application.Pipelines.Caching
{
    public class CacheSettings
    {
        public bool UseRedis { get; set; }
        public string? RedisConfiguration { get; set; }
        public int SlidingExpiration { get; set; }
    }
}
