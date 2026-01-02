using Core.Application.Pipelines.Authorization.Constants;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Pipelines.Authorization
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ISecuredRequest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                throw new AuthorizationException("You are not logged in!");

            // If no roles are required for the command or query, proceed to the next behavior
            if (request.Roles.Length == 0)
                return await next();

            // Get user roles
            List<string>? userRoleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();

            if (userRoleClaims == null || userRoleClaims.Count == 0)
                throw new AuthorizationException("You are not authenticated - You don't have any roles assigned.");

            // Check if user is admin (bypass other role checks)
            bool isAdmin = userRoleClaims.Any(role =>
                role.Equals(GeneralOperationClaims.Admin, StringComparison.OrdinalIgnoreCase)
            );

            if (isAdmin)
                return await next();

            // 5. Check if user has any of the required roles
            bool hasRequiredRole = request.Roles.Any(requiredRole =>
                userRoleClaims.Any(userRole =>
                    userRole.Equals(requiredRole, StringComparison.OrdinalIgnoreCase)
                )
            );

            if (!hasRequiredRole)
            {
                string requiredRoles = string.Join(", ", request.Roles);
                throw new AuthorizationException($"You need one of these roles: {requiredRoles}");
            }

            // 6. User is authorized, proceed to next behavior
            TResponse response = await next();
            return response;
        }
    }
}
