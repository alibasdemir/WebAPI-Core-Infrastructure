using Application.Features.UserOperationClaims.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Pipelines.Logging;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.UserOperationClaims.Commands.SoftDelete
{
    public class SoftDeleteUserOperationClaimCommand : IRequest<SoftDeleteUserOperationClaimResponseDTO>, ISecuredRequest, ILoggableRequest
    {
        public int Id { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class SoftDeleteUserOperationClaimCommandHandler : IRequestHandler<SoftDeleteUserOperationClaimCommand, SoftDeleteUserOperationClaimResponseDTO>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

            public SoftDeleteUserOperationClaimCommandHandler(
                IUserOperationClaimRepository userOperationClaimRepository,
                IMapper mapper,
                UserOperationClaimBusinessRules userOperationClaimBusinessRules)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            }

            public async Task<SoftDeleteUserOperationClaimResponseDTO> Handle(SoftDeleteUserOperationClaimCommand request, CancellationToken cancellationToken)
            {
                // Business rules validation
                await _userOperationClaimBusinessRules.UserOperationClaimShouldExistWhenSelected(request.Id);

                // Get existing user operation claim
                UserOperationClaim? userOperationClaim = await _userOperationClaimRepository.GetAsync(
                    predicate: uoc => uoc.Id == request.Id && !uoc.IsDeleted,
                    enableTracking: true,
                    cancellationToken: cancellationToken
                );

                _userOperationClaimBusinessRules.UserOperationClaimShouldNotBeDeleted(userOperationClaim!);

                // Soft delete
                await _userOperationClaimRepository.SoftDeleteAsync(userOperationClaim!);

                SoftDeleteUserOperationClaimResponseDTO response = _mapper.Map<SoftDeleteUserOperationClaimResponseDTO>(userOperationClaim);
                return response;
            }
        }
    }
}