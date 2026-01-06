using Application.Features.UserOperationClaims.Rules;
using Application.Repositories;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Pipelines.Logging;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.UserOperationClaims.Commands.AssignMultiple
{
    public class AssignMultipleOperationClaimsToUserCommand : IRequest<AssignMultipleOperationClaimsToUserResponseDTO>, ISecuredRequest, ILoggableRequest
    {
        public int UserId { get; set; }
        public List<int> OperationClaimIds { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class AssignMultipleOperationClaimsToUserCommandHandler : IRequestHandler<AssignMultipleOperationClaimsToUserCommand, AssignMultipleOperationClaimsToUserResponseDTO>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

            public AssignMultipleOperationClaimsToUserCommandHandler(
                IUserOperationClaimRepository userOperationClaimRepository,
                UserOperationClaimBusinessRules userOperationClaimBusinessRules)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            }

            public async Task<AssignMultipleOperationClaimsToUserResponseDTO> Handle(AssignMultipleOperationClaimsToUserCommand request, CancellationToken cancellationToken)
            {
                // Validate user exists
                await _userOperationClaimBusinessRules.UserShouldExist(request.UserId);

                int assignedCount = 0;
                int skippedCount = 0;
                List<string> errors = new();

                foreach (var operationClaimId in request.OperationClaimIds)
                {
                    try
                    {
                        // Check if operation claim exists
                        await _userOperationClaimBusinessRules.OperationClaimShouldExist(operationClaimId);

                        // Check if user already has this claim
                        bool alreadyHas = await _userOperationClaimRepository.AnyAsync(
                            uoc => uoc.UserId == request.UserId && uoc.OperationClaimId == operationClaimId && !uoc.IsDeleted
                        );

                        if (alreadyHas)
                        {
                            skippedCount++;
                            continue;
                        }

                        // Assign operation claim
                        UserOperationClaim userOperationClaim = new()
                        {
                            UserId = request.UserId,
                            OperationClaimId = operationClaimId,
                            CreatedDate = DateTime.UtcNow,
                            IsDeleted = false
                        };

                        await _userOperationClaimRepository.AddAsync(userOperationClaim);
                        assignedCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Operation claim ID {operationClaimId}: {ex.Message}");
                    }
                }

                AssignMultipleOperationClaimsToUserResponseDTO response = new()
                {
                    UserId = request.UserId,
                    AssignedCount = assignedCount,
                    SkippedCount = skippedCount,
                    Errors = errors,
                    Message = $"{assignedCount} operation claim(s) assigned, {skippedCount} skipped."
                };

                return response;
            }
        }
    }
}