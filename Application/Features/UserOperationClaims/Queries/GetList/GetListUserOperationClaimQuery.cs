using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Dynamic;
using Core.Pagination;
using Core.Pagination.Requests;
using Core.Security.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserOperationClaims.Queries.GetList
{
    public class GetListUserOperationClaimQuery : IRequest<GetListUserOperationClaimResponseDTO>, ISecuredRequest
    {
        public PageRequest PageRequest { get; set; } = new();
        public DynamicQuery? Dynamic { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class GetListUserOperationClaimQueryHandler : IRequestHandler<GetListUserOperationClaimQuery, GetListUserOperationClaimResponseDTO>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;

            public GetListUserOperationClaimQueryHandler(
                IUserOperationClaimRepository userOperationClaimRepository,
                IMapper mapper)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
            }

            public async Task<GetListUserOperationClaimResponseDTO> Handle(GetListUserOperationClaimQuery request, CancellationToken cancellationToken)
            {
                IPaginate<UserOperationClaim> userOperationClaims;

                if (request.Dynamic != null)
                {
                    userOperationClaims = await _userOperationClaimRepository.GetListByDynamicAsync(
                        dynamic: request.Dynamic,
                        predicate: uoc => !uoc.IsDeleted,
                        include: query => query.Include(uoc => uoc.User).Include(uoc => uoc.OperationClaim),
                        index: request.PageRequest.Index,
                        size: request.PageRequest.Size,
                        enableTracking: false,
                        cancellationToken: cancellationToken
                    );
                }
                else
                {
                    userOperationClaims = await _userOperationClaimRepository.GetListAsync(
                        predicate: uoc => !uoc.IsDeleted,
                        include: query => query.Include(uoc => uoc.User).Include(uoc => uoc.OperationClaim),
                        orderBy: query => query.OrderByDescending(uoc => uoc.CreatedDate),
                        index: request.PageRequest.Index,
                        size: request.PageRequest.Size,
                        enableTracking: false,
                        cancellationToken: cancellationToken
                    );
                }

                GetListUserOperationClaimResponseDTO response = _mapper.Map<GetListUserOperationClaimResponseDTO>(userOperationClaims);
                return response;
            }
        }
    }
}