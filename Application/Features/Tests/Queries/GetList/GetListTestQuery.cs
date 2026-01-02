using Application.Repositories;
using AutoMapper;
using Core.Dynamic;
using Core.Pagination;
using Core.Pagination.Requests;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Queries.GetList
{
    public class GetListTestQuery : IRequest<GetListTestResponseDTO>
    {
        public PageRequest PageRequest { get; set; } = new();
        public DynamicQuery? Dynamic { get; set; }

        public class GetListTestQueryHandler : IRequestHandler<GetListTestQuery, GetListTestResponseDTO>
        {
            private readonly ITestRepository _testRepository;
            private readonly IMapper _mapper;

            public GetListTestQueryHandler(ITestRepository testRepository, IMapper mapper)
            {
                _testRepository = testRepository;
                _mapper = mapper;
            }

            public async Task<GetListTestResponseDTO> Handle(GetListTestQuery request, CancellationToken cancellationToken)
            {
                IPaginate<Test> tests;

                // If dynamic query is provided, use it
                if (request.Dynamic != null)
                {
                    tests = await _testRepository.GetListByDynamicAsync(
                        dynamic: request.Dynamic,
                        predicate: t => !t.IsDeleted,
                        index: request.PageRequest.Index,
                        size: request.PageRequest.Size,
                        enableTracking: false,
                        cancellationToken: cancellationToken
                    );
                }
                else
                {
                    // Default ordering by CreatedDate descending
                    tests = await _testRepository.GetListAsync(
                        predicate: t => !t.IsDeleted,
                        orderBy: query => query.OrderByDescending(t => t.CreatedDate),
                        index: request.PageRequest.Index,
                        size: request.PageRequest.Size,
                        enableTracking: false,
                        cancellationToken: cancellationToken
                    );
                }

                GetListTestResponseDTO response = _mapper.Map<GetListTestResponseDTO>(tests);
                return response;
            }
        }
    }
}