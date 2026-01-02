using Application.Features.OperationClaims.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.OperationClaims.Commands.Update
{
    public class UpdateOperationClaimCommand : IRequest<UpdateOperationClaimResponseDTO>, ISecuredRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class UpdateOperationClaimCommandHandler : IRequestHandler<UpdateOperationClaimCommand, UpdateOperationClaimResponseDTO>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly IMapper _mapper;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public UpdateOperationClaimCommandHandler(
                IOperationClaimRepository operationClaimRepository,
                IMapper mapper,
                OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _operationClaimRepository = operationClaimRepository;
                _mapper = mapper;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<UpdateOperationClaimResponseDTO> Handle(UpdateOperationClaimCommand request, CancellationToken cancellationToken)
            {
                // Business rules validation
                await _operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(request.Id);
                _operationClaimBusinessRules.OperationClaimNameShouldBeValid(request.Name);
                await _operationClaimBusinessRules.OperationClaimNameShouldBeUniqueForUpdate(request.Name, request.Id);

                // Get existing operation claim
                OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(
                    predicate: oc => oc.Id == request.Id,
                    enableTracking: true,
                    cancellationToken: cancellationToken
                );

                _operationClaimBusinessRules.OperationClaimShouldNotBeDeleted(operationClaim!);

                // Update properties
                operationClaim!.Name = request.Name;
                operationClaim.UpdatedDate = DateTime.UtcNow;

                await _operationClaimRepository.UpdateAsync(operationClaim);

                UpdateOperationClaimResponseDTO response = _mapper.Map<UpdateOperationClaimResponseDTO>(operationClaim);
                return response;
            }
        }
    }
}