using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Logging;
using Core.Pagination;
using Core.Pagination.Requests;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Queries.Search
{
    public class SearchTestsQuery : IRequest<SearchTestsResponseDTO>, ILoggableRequest
    {
        public string? SearchTerm { get; set; }
        public PageRequest PageRequest { get; set; } = new();

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
                    index: request.PageRequest.Index,
                    size: request.PageRequest.Size,
                    enableTracking: false,
                    cancellationToken: cancellationToken
                );

                SearchTestsResponseDTO response = _mapper.Map<SearchTestsResponseDTO>(tests);
                return response;
            }
        }
    }
}