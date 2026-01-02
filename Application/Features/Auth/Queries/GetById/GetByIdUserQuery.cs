using Application.Features.Auth.Queries.GetByIdUser;
using Application.Features.Auth.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Queries.GetById
{
    public class GetByIdUserQuery : IRequest<GetByIdUserResponseDTO>, ISecuredRequest
    {
        public int Id { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, GetByIdUserResponseDTO>
        {
            private readonly IUserRepository _userRepository;
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly AuthBusinessRules _authBusinessRules;

            public GetByIdUserQueryHandler(
                IUserRepository userRepository,
                IUserOperationClaimRepository userOperationClaimRepository,
                IMapper mapper,
                AuthBusinessRules authBusinessRules)
            {
                _userRepository = userRepository;
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _authBusinessRules = authBusinessRules;
            }

            public async Task<GetByIdUserResponseDTO> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
            {
                // Get user
                User? user = await _userRepository.GetAsync(
                    predicate: u => u.Id == request.Id && !u.IsDeleted,
                    enableTracking: false,
                    cancellationToken: cancellationToken
                );

                _authBusinessRules.CheckEntityExists(user);

                // Get user roles
                var userRoles = await _userOperationClaimRepository
                    .Query()
                    .AsNoTracking()
                    .Where(uoc => uoc.UserId == user!.Id && !uoc.IsDeleted)
                    .Include(uoc => uoc.OperationClaim)
                    .Select(uoc => uoc.OperationClaim.Name)
                    .ToListAsync(cancellationToken);

                GetByIdUserResponseDTO response = _mapper.Map<GetByIdUserResponseDTO>(user);
                response.Roles = userRoles;

                return response;
            }
        }
    }
}