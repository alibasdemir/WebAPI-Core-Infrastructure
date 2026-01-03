using Core.Application.Services.Email;
using Infrastructure.Services.Email;
using Infrastructure.Services.Email.MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Email Service Configuration
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

            services.AddScoped<IEmailService, MailKitEmailService>();

            return services;
        }
    }
}
