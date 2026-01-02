using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Pagination;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Queries.GetListUser
{
    public class GetListUserQuery : IRequest<GetListUserResponseDTO>, ISecuredRequest
    {
        public int Index { get; set; } = 0;
        public int Size { get; set; } = 10;
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class GetListUserQueryHandler : IRequestHandler<GetListUserQuery, GetListUserResponseDTO>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public GetListUserQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<GetListUserResponseDTO> Handle(GetListUserQuery request, CancellationToken cancellationToken)
            {
                IPaginate<User> users = await _userRepository.GetListAsync(
                    predicate: u => !u.IsDeleted,
                    orderBy: query => query.OrderByDescending(u => u.CreatedDate),
                    index: request.Index,
                    size: request.Size,
                    enableTracking: false,
                    cancellationToken: cancellationToken
                );

                GetListUserResponseDTO response = _mapper.Map<GetListUserResponseDTO>(users);
                return response;
            }
        }
    }
}