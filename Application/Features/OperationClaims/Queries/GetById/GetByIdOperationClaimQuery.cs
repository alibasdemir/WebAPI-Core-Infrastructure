using Application.Features.OperationClaims.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Pipelines.Logging;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.OperationClaims.Queries.GetById
{
    public class GetByIdOperationClaimQuery : IRequest<GetByIdOperationClaimResponseDTO>, ISecuredRequest, ILoggableRequest
    {
        public int Id { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class GetByIdOperationClaimQueryHandler : IRequestHandler<GetByIdOperationClaimQuery, GetByIdOperationClaimResponseDTO>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly IMapper _mapper;
            private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

            public GetByIdOperationClaimQueryHandler(
                IOperationClaimRepository operationClaimRepository,
                IMapper mapper,
                OperationClaimBusinessRules operationClaimBusinessRules)
            {
                _operationClaimRepository = operationClaimRepository;
                _mapper = mapper;
                _operationClaimBusinessRules = operationClaimBusinessRules;
            }

            public async Task<GetByIdOperationClaimResponseDTO> Handle(GetByIdOperationClaimQuery request, CancellationToken cancellationToken)
            {
                OperationClaim? operationClaim = await _operationClaimRepository.GetAsync(
                    predicate: oc => oc.Id == request.Id && !oc.IsDeleted,
                    enableTracking: false,
                    cancellationToken: cancellationToken
                );

                _operationClaimBusinessRules.CheckEntityExists(operationClaim);

                GetByIdOperationClaimResponseDTO response = _mapper.Map<GetByIdOperationClaimResponseDTO>(operationClaim);
                return response;
            }
        }
    }
}