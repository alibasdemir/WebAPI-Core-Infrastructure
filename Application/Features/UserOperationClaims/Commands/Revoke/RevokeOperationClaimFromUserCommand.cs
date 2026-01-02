using Application.Features.UserOperationClaims.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.UserOperationClaims.Commands.Revoke
{
    public class RevokeOperationClaimFromUserCommand : IRequest<RevokeOperationClaimFromUserResponseDTO>, ISecuredRequest
    {
        public int Id { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class RevokeOperationClaimFromUserCommandHandler : IRequestHandler<RevokeOperationClaimFromUserCommand, RevokeOperationClaimFromUserResponseDTO>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

            public RevokeOperationClaimFromUserCommandHandler(
                IUserOperationClaimRepository userOperationClaimRepository,
                IMapper mapper,
                UserOperationClaimBusinessRules userOperationClaimBusinessRules)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            }

            public async Task<RevokeOperationClaimFromUserResponseDTO> Handle(RevokeOperationClaimFromUserCommand request, CancellationToken cancellationToken)
            {
                // Business rules validation
                await _userOperationClaimBusinessRules.UserOperationClaimShouldExistWhenSelected(request.Id);

                // Get existing user operation claim
                UserOperationClaim? userOperationClaim = await _userOperationClaimRepository.GetAsync(
                    predicate: uoc => uoc.Id == request.Id,
                    enableTracking: true,
                    cancellationToken: cancellationToken
                );

                // Hard delete
                await _userOperationClaimRepository.DeleteAsync(userOperationClaim!);

                RevokeOperationClaimFromUserResponseDTO response = _mapper.Map<RevokeOperationClaimFromUserResponseDTO>(userOperationClaim);
                return response;
            }
        }
    }
}