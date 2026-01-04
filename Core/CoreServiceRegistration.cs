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

            return services;
        }
    }
}
