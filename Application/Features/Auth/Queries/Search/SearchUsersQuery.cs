using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Pipelines.Logging;
using Core.Pagination;
using Core.Pagination.Requests;
using Core.Security.Entities;
using MediatR;

namespace Application.Features.Auth.Queries.SearchUsers
{
    public class SearchUsersQuery : IRequest<SearchUsersResponseDTO>, ISecuredRequest, ILoggableRequest
    {
        public string? SearchTerm { get; set; }
        public PageRequest PageRequest { get; set; } = new();
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, SearchUsersResponseDTO>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public SearchUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<SearchUsersResponseDTO> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
            {
                IPaginate<User> users = await _userRepository.GetListAsync(
                    predicate: u => !u.IsDeleted &&
                                   (string.IsNullOrEmpty(request.SearchTerm) ||
                                    u.UserName.Contains(request.SearchTerm) ||
                                    u.FirstName.Contains(request.SearchTerm) ||
                                    u.LastName.Contains(request.SearchTerm) ||
                                    u.Email.Contains(request.SearchTerm)),
                    orderBy: query => query.OrderByDescending(u => u.CreatedDate),
                    index: request.PageRequest.Index,
                    size: request.PageRequest.Size,
                    enableTracking: false,
                    cancellationToken: cancellationToken
                );

                SearchUsersResponseDTO response = _mapper.Map<SearchUsersResponseDTO>(users);
                return response;
            }
        }
    }
}