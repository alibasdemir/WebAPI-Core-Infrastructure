using Application.Features.Tests.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Performance;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Queries.GetById
{
    public class GetByIdTestQuery : IRequest<GetByIdTestResponseDTO>, ILoggableRequest, ICachableRequest, IIntervalRequest
    {
        public int Id { get; set; }

        public string CacheKey => $"GetByIdTest-{Id}";
        public string? CacheGroupKey => "GetTests";
        public bool BypassCache { get; set; } // default: false. dont want true cause cache will be bypassed. or can be used -> public bool BypassCache => false;
        public TimeSpan? SlidingExpiration { get; set; } // or public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(30);
        public int Interval => 500;

        public class GetByIdTestQueryHandler : IRequestHandler<GetByIdTestQuery, GetByIdTestResponseDTO>
        {
            private readonly ITestRepository _testRepository;
            private readonly IMapper _mapper;
            private readonly TestBusinessRules _testBusinessRules;

            public GetByIdTestQueryHandler(
                ITestRepository testRepository,
                IMapper mapper,
                TestBusinessRules testBusinessRules)
            {
                _testRepository = testRepository;
                _mapper = mapper;
                _testBusinessRules = testBusinessRules;
            }

            public async Task<GetByIdTestResponseDTO> Handle(GetByIdTestQuery request, CancellationToken cancellationToken)
            {
                // Get test
                Test? test = await _testRepository.GetAsync(
                    predicate: t => t.Id == request.Id && !t.IsDeleted,
                    enableTracking: false,
                    cancellationToken: cancellationToken
                );

                _testBusinessRules.CheckEntityExists(test);

                GetByIdTestResponseDTO response = _mapper.Map<GetByIdTestResponseDTO>(test);
                return response;
            }
        }
    }
}