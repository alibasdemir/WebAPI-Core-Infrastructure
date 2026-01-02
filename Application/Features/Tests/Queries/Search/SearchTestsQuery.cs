using Application.Repositories;
using AutoMapper;
using Core.Pagination;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Queries.Search
{
    public class SearchTestsQuery : IRequest<SearchTestsResponseDTO>
    {
        public string? SearchTerm { get; set; }
        public int Index { get; set; } = 0;
        public int Size { get; set; } = 10;

        public class SearchTestsQueryHandler : IRequestHandler<SearchTestsQuery, SearchTestsResponseDTO>
        {
            private readonly ITestRepository _testRepository;
            private readonly IMapper _mapper;

            public SearchTestsQueryHandler(ITestRepository testRepository, IMapper mapper)
            {
                _testRepository = testRepository;
                _mapper = mapper;
            }

            public async Task<SearchTestsResponseDTO> Handle(SearchTestsQuery request, CancellationToken cancellationToken)
            {
                IPaginate<Test> tests = await _testRepository.GetListAsync(
                    predicate: t => !t.IsDeleted &&
                                   (string.IsNullOrEmpty(request.SearchTerm) ||
                                    t.Name.Contains(request.SearchTerm)),
                    orderBy: query => query.OrderByDescending(t => t.CreatedDate),
                    index: request.Index,
                    size: request.Size,
                    enableTracking: false,
                    cancellationToken: cancellationToken
                );

                SearchTestsResponseDTO response = _mapper.Map<SearchTestsResponseDTO>(tests);
                return response;
            }
        }
    }
}