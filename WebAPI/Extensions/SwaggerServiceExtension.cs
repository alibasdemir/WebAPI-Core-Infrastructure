using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebAPI.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerWithJwtAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Modular Web API Core Infrastructure",
                    Version = "v1.0.0",
                    Description = "A comprehensive implementation of a **Modular and Reusable Web API Core Infrastructure** using ASP.NET Core.\n\n" +
                                  "This API serves as a robust foundational template designed with **Clean Architecture** principles, ensuring scalability, maintainability, and testability.\n\n" +
                                  "### 🚀 Key Features\n" +
                                  "- **Architecture:** Clean Architecture (Core, Application, Infrastructure, Persistence, Domain)\n" +
                                  "- **Security:** JWT-based Authentication & RBAC Authorization\n" +
                                  "- **Pattern:** CQRS with MediatR & Generic Repository Pattern\n" +
                                  "- **Mapping:** Efficient object-to-object mapping with AutoMapper\n" +
                                  "- **Quality:** Global Exception Handling & FluentValidation\n" +
                                  "- **Performance:** Optimized Data Access & Caching mechanisms\n\n" +
                                  "### 🛠️ Getting Started\n" +
                                  "1. **Register:** Use `POST /api/Auths/Register` to create an account.\n" +
                                  "2. **Login:** Use `POST /api/Auths/Login` to obtain a JWT `Bearer` token.\n" +
                                  "3. **Authorize:** Click the `Authorize` button (top right) and **paste your token only** (e.g., `eyJ...`).\n" +
                                  "4. **Explore:** Access protected endpoints securely.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ali Başdemir",
                        Email = "alibasdemir@gmail.com",
                        Url = new Uri("https://github.com/alibasdemir")
                    }
                });

                // JWT Bearer Configuration
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // This setting allows Swagger to automatically add a "Bearer".
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your valid JWT token below.\nExample: `eyJ...` (Just paste the token, do not add 'Bearer ' prefix)"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
            });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerWithUi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Modular API v1");
                opt.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }
}
