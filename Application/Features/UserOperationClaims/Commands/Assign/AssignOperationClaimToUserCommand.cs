using Application.Features.UserOperationClaims.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.UserOperationClaims.Commands.Assign
{
    public class AssignOperationClaimToUserCommand : IRequest<AssignOperationClaimToUserResponseDTO>, ISecuredRequest
    {
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class AssignOperationClaimToUserCommandHandler : IRequestHandler<AssignOperationClaimToUserCommand, AssignOperationClaimToUserResponseDTO>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;
            private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

            public AssignOperationClaimToUserCommandHandler(
                IUserOperationClaimRepository userOperationClaimRepository,
                IMapper mapper,
                UserOperationClaimBusinessRules userOperationClaimBusinessRules)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
                _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            }

            public async Task<AssignOperationClaimToUserResponseDTO> Handle(AssignOperationClaimToUserCommand request, CancellationToken cancellationToken)
            {
                // Business rules validation
                await _userOperationClaimBusinessRules.UserShouldExist(request.UserId);
                await _userOperationClaimBusinessRules.OperationClaimShouldExist(request.OperationClaimId);
                await _userOperationClaimBusinessRules.UserShouldNotHaveOperationClaim(request.UserId, request.OperationClaimId);

                // Create user operation claim
                UserOperationClaim userOperationClaim = new()
                {
                    UserId = request.UserId,
                    OperationClaimId = request.OperationClaimId,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _userOperationClaimRepository.AddAsync(userOperationClaim);

                AssignOperationClaimToUserResponseDTO response = _mapper.Map<AssignOperationClaimToUserResponseDTO>(userOperationClaim);
                return response;
            }
        }
    }
}