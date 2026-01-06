using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Pipelines.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserOperationClaims.Queries.GetByUserId
{
    public class GetUserOperationClaimsByUserIdQuery : IRequest<GetUserOperationClaimsByUserIdResponseDTO>, ISecuredRequest, ILoggableRequest
    {
        public int UserId { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class GetUserOperationClaimsByUserIdQueryHandler : IRequestHandler<GetUserOperationClaimsByUserIdQuery, GetUserOperationClaimsByUserIdResponseDTO>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;

            public GetUserOperationClaimsByUserIdQueryHandler(
                IUserOperationClaimRepository userOperationClaimRepository,
                IMapper mapper)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
            }

            public async Task<GetUserOperationClaimsByUserIdResponseDTO> Handle(GetUserOperationClaimsByUserIdQuery request, CancellationToken cancellationToken)
            {
                var userOperationClaims = await _userOperationClaimRepository
                    .Query()
                    .AsNoTracking()
                    .Where(uoc => uoc.UserId == request.UserId && !uoc.IsDeleted)
                    .Include(uoc => uoc.OperationClaim)
                    .Include(uoc => uoc.User)
                    .Select(uoc => new UserOperationClaimItemDTO
                    {
                        Id = uoc.Id,
                        UserId = uoc.UserId,
                        UserName = uoc.User.UserName,
                        OperationClaimId = uoc.OperationClaimId,
                        OperationClaimName = uoc.OperationClaim.Name,
                        CreatedDate = uoc.CreatedDate
                    })
                    .ToListAsync(cancellationToken);

                GetUserOperationClaimsByUserIdResponseDTO response = new()
                {
                    UserId = request.UserId,
                    Items = userOperationClaims
                };

                return response;
            }
        }
    }
}