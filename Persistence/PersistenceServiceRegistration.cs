using Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            // Register your persistence services here, e.g. DbContext, Repositories, etc.
            // services.AddDbContext<YourDbContext>(options => ...);
            // services.AddScoped<IYourRepository, YourRepository>();
            services.AddDbContext<BaseDbContext>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
