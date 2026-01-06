using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Pipelines.Logging;
using Core.Dynamic;
using Core.Pagination;
using Core.Pagination.Requests;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.OperationClaims.Queries.GetList
{
    public class GetListOperationClaimQuery : IRequest<GetListOperationClaimResponseDTO>, ISecuredRequest, ILoggableRequest
    {
        public PageRequest PageRequest { get; set; } = new();
        public DynamicQuery? Dynamic { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class GetListOperationClaimQueryHandler : IRequestHandler<GetListOperationClaimQuery, GetListOperationClaimResponseDTO>
        {
            private readonly IOperationClaimRepository _operationClaimRepository;
            private readonly IMapper _mapper;

            public GetListOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository, IMapper mapper)
            {
                _operationClaimRepository = operationClaimRepository;
                _mapper = mapper;
            }

            public async Task<GetListOperationClaimResponseDTO> Handle(GetListOperationClaimQuery request, CancellationToken cancellationToken)
            {
                IPaginate<OperationClaim> operationClaims;

                if (request.Dynamic != null)
                {
                    operationClaims = await _operationClaimRepository.GetListByDynamicAsync(
                        dynamic: request.Dynamic,
                        predicate: oc => !oc.IsDeleted,
                        index: request.PageRequest.Index,
                        size: request.PageRequest.Size,
                        enableTracking: false,
                        cancellationToken: cancellationToken
                    );
                }
                else
                {
                    operationClaims = await _operationClaimRepository.GetListAsync(
                        predicate: oc => !oc.IsDeleted,
                        orderBy: query => query.OrderBy(oc => oc.Name),
                        index: request.PageRequest.Index,
                        size: request.PageRequest.Size,
                        enableTracking: false,
                        cancellationToken: cancellationToken
                    );
                }

                GetListOperationClaimResponseDTO response = _mapper.Map<GetListOperationClaimResponseDTO>(operationClaims);
                return response;
            }
        }
    }
}