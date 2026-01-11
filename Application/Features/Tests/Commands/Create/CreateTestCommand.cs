using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Performance;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Commands.Create
{
    public class CreateTestCommand : IRequest<CreateTestResponseDTO>, ISecuredRequest, ILoggableRequest, ICacheRemoverRequest, IIntervalRequest
    {
        public string Name { get; set; }
        public string[] Roles => ["test.create"];
        public bool BypassCache => false;
        public string? CacheKey => null;
        public string CacheGroupKey => "GetTests";
        public int Interval => 1000;

        public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, CreateTestResponseDTO>
        {
            private readonly ITestRepository _testRepository;
            private readonly IMapper _mapper;

            public CreateTestCommandHandler(ITestRepository testRepository, IMapper mapper)
            {
                _testRepository = testRepository;
                _mapper = mapper;
            }

            public async Task<CreateTestResponseDTO> Handle(CreateTestCommand request, CancellationToken cancellationToken)
            {
                Test test = _mapper.Map<Test>(request);
                await _testRepository.AddAsync(test);

                return _mapper.Map<CreateTestResponseDTO>(test);
            }
        }
    }
}
