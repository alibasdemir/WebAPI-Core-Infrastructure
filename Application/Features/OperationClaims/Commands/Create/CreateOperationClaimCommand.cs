using Application.Features.OperationClaims.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.OperationClaims.Commands.Create
{
    public class CreateOperationClaimCommand : IRequest<CreateOperationClaimResponseDTO>, ISecuredRequest
    {
        public string Name { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommand, CreateOperationClaimResponseDTO>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly IMapper _mapper;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public CreateOperationClaimCommandHandler(
                IOperationClaimRepository operationClaimRepository,
                IMapper mapper,
                OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _operationClaimRepository = operationClaimRepository;
                _mapper = mapper;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<CreateOperationClaimResponseDTO> Handle(CreateOperationClaimCommand request, CancellationToken cancellationToken)
            {
                // Business rules validation
                _operationClaimBusinessRules.OperationClaimNameShouldBeValid(request.Name);
                await _operationClaimBusinessRules.OperationClaimNameShouldBeUnique(request.Name);

                // Create operation claim
                OperationClaim operationClaim = _mapper.Map<OperationClaim>(request);
                operationClaim.CreatedDate = DateTime.UtcNow;
                operationClaim.IsDeleted = false;

                await _operationClaimRepository.AddAsync(operationClaim);

                CreateOperationClaimResponseDTO response = _mapper.Map<CreateOperationClaimResponseDTO>(operationClaim);
                return response;
            }
        }
    }
}