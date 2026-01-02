using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserOperationClaims.Queries.GetUsersByOperationClaimId
{
    public class GetUsersByOperationClaimIdQuery : IRequest<GetUsersByOperationClaimIdResponseDTO>, ISecuredRequest
    {
        public int OperationClaimId { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class GetUsersByOperationClaimIdQueryHandler : IRequestHandler<GetUsersByOperationClaimIdQuery, GetUsersByOperationClaimIdResponseDTO>
        {
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;
            private readonly IMapper _mapper;

            public GetUsersByOperationClaimIdQueryHandler(
                IUserOperationClaimRepository userOperationClaimRepository,
                IMapper mapper)
            {
                _userOperationClaimRepository = userOperationClaimRepository;
                _mapper = mapper;
            }

            public async Task<GetUsersByOperationClaimIdResponseDTO> Handle(GetUsersByOperationClaimIdQuery request, CancellationToken cancellationToken)
            {
                var users = await _userOperationClaimRepository
                    .Query()
                    .AsNoTracking()
                    .Where(uoc => uoc.OperationClaimId == request.OperationClaimId && !uoc.IsDeleted)
                    .Include(uoc => uoc.User)
                    .Select(uoc => new UserInfoDTO
                    {
                        Id = uoc.User.Id,
                        UserName = uoc.User.UserName,
                        Email = uoc.User.Email,
                        FirstName = uoc.User.FirstName,
                        LastName = uoc.User.LastName,
                        AssignedDate = uoc.CreatedDate
                    })
                    .ToListAsync(cancellationToken);

                GetUsersByOperationClaimIdResponseDTO response = new()
                {
                    OperationClaimId = request.OperationClaimId,
                    TotalUsers = users.Count,
                    Users = users
                };

                return response;
            }
        }
    }
}