using Application.Features.Auth.Rules;
using Application.Features.OperationClaims.Rules;
using Application.Features.Tests.Rules;
using Application.Features.UserOperationClaims.Rules;
using Application.Services.AuthService;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.CrossCuttingConcerns.Logging.Serilog.Logger;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                config.AddOpenBehavior(typeof(CachingBehavior<,>));
                config.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddSingleton<LoggerServiceBase, FileLogger>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<AuthBusinessRules>();
            services.AddScoped<TestBusinessRules>();
            services.AddScoped<OperationClaimBusinessRules>();
            services.AddScoped<UserOperationClaimBusinessRules>();

            return services;
        }
    }
}
