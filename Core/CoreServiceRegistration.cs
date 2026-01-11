using Core.Application.Pipelines.Caching;
using Core.Application.Services.UserEmailContent;
using Core.Security.JWT;
using Core.Security.UserTokenGeneration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IUserTokenGeneratorService, UserTokenGeneratorService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IEmailLinkService, EmailLinkService>();

            // Configure token expiration options from appsettings
            services.Configure<UserTokenExpirationOptions>(options =>
                configuration.GetSection("UserTokenExpirationOptions").Bind(options));

            services.Configure<CacheSettings>(
                configuration.GetSection("CacheSettings"));

            // Cache Configuration
            var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>();

            if (cacheSettings?.UseRedis == true)
            {
                try
                {
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = cacheSettings.RedisConfiguration;
                        options.InstanceName = "webApiCoreInfrastructure_";
                    });

                    Console.WriteLine("Redis Distributed Cache activated");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Redis connection failed: {ex.Message}");
                    Console.WriteLine("Falling back to Memory Distributed Cache");

                    services.AddDistributedMemoryCache();
                }
            }
            else
            {
                services.AddDistributedMemoryCache();

                Console.WriteLine("Memory Distributed Cache activated");
            }

            return services;
        }
    }
}
