using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Pipelines.Logging;
using Core.Pagination;
using Core.Pagination.Requests;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Queries.GetListUser
{
    public class GetListUserQuery : IRequest<GetListUserResponseDTO>, ISecuredRequest, ILoggableRequest
    {
        public PageRequest PageRequest { get; set; } = new();
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
                    index: request.PageRequest.Index,
                    size: request.PageRequest.Size,
                    enableTracking: false,
                    cancellationToken: cancellationToken
                );

                GetListUserResponseDTO response = _mapper.Map<GetListUserResponseDTO>(users);
                return response;
            }
        }
    }
}