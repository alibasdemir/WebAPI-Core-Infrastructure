using Application.Features.OperationClaims.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.OperationClaims.Commands.Delete
{
    public class DeleteOperationClaimCommand : IRequest<DeleteOperationClaimResponseDTO>, ISecuredRequest
    {
        public int Id { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class DeleteOperationClaimCommandHandler : IRequestHandler<DeleteOperationClaimCommand, DeleteOperationClaimResponseDTO>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly IMapper _mapper;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public DeleteOperationClaimCommandHandler(
                IOperationClaimRepository operationClaimRepository,
                IMapper mapper,
                OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _operationClaimRepository = operationClaimRepository;
                _mapper = mapper;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<DeleteOperationClaimResponseDTO> Handle(DeleteOperationClaimCommand request, CancellationToken cancellationToken)
            {
                // Business rules validation
                await _operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(request.Id);
                await _operationClaimBusinessRules.OperationClaimShouldNotHaveUsers(request.Id);

                // Get existing operation claim
                OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(
                    predicate: oc => oc.Id == request.Id,
                    enableTracking: true,
                    cancellationToken: cancellationToken
                );

                // Hard delete
                await _operationClaimRepository.DeleteAsync(operationClaim!);

                DeleteOperationClaimResponseDTO response = _mapper.Map<DeleteOperationClaimResponseDTO>(operationClaim);
                return response;
            }
        }
    }
}