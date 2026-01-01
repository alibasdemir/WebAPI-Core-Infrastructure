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

            List<string>? userRoleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();

            if (userRoleClaims == null)
                throw new AuthorizationException("You are not authenticated.");

            bool isNotMatchedAUserRoleClaimWithRequestRoles = string.IsNullOrEmpty(
                userRoleClaims.FirstOrDefault(
                    userRoleClaim => 
                    userRoleClaim.Equals(GeneralOperationClaims.Admin, StringComparison.OrdinalIgnoreCase) 
                    ||
                    request.Roles.Any(role => role.Equals(userRoleClaim, StringComparison.OrdinalIgnoreCase)))
            );

            if (isNotMatchedAUserRoleClaimWithRequestRoles)
                throw new AuthorizationException("You are not authorized.");

            TResponse response = await next();
            return response;
        }
    }
}
