using Application.Features.Auth.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Logging;
using Core.Security.Entities;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<GetCurrentUserResponseDTO>, ILoggableRequest
    {
        public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponseDTO>
        {
            private readonly IUserRepository _userRepository;
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;
            private readonly AuthBusinessRules _authBusinessRules;

            public GetCurrentUserQueryHandler(
                IUserRepository userRepository,
                IUserOperationClaimRepository userOperationClaimRepository,
                IHttpContextAccessor httpContextAccessor,
                IMapper mapper,
                AuthBusinessRules authBusinessRules)
            {
                _userRepository = userRepository;
                _userOperationClaimRepository = userOperationClaimRepository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
            }

            public async Task<GetCurrentUserResponseDTO> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
            {
                // Get user ID from JWT token
                int userId = _httpContextAccessor.HttpContext.User.GetUserId();

                // Get user
                User? user = await _userRepository.GetAsync(
                    predicate: u => u.Id == userId && !u.IsDeleted,
                    enableTracking: false,
                    cancellationToken: cancellationToken
                );

                _authBusinessRules.CheckEntityExists(user);

                // Get user roles
                var userRoles = await _userOperationClaimRepository
                    .Query()
                    .AsNoTracking()
                    .Where(uoc => uoc.UserId == userId && !uoc.IsDeleted)
                    .Include(uoc => uoc.OperationClaim)
                    .Select(uoc => uoc.OperationClaim.Name)
                    .ToListAsync(cancellationToken);

                GetCurrentUserResponseDTO response = _mapper.Map<GetCurrentUserResponseDTO>(user);
                response.Roles = userRoles;

                return response;
            }
        }
    }
}