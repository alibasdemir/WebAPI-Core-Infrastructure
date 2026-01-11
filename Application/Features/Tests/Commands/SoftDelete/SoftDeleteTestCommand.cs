using Application.Features.Tests.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Performance;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Commands.SoftDelete
{
    public class SoftDeleteTestCommand : IRequest<SoftDeleteTestResponseDTO>, ISecuredRequest, ILoggableRequest, ICacheRemoverRequest, IIntervalRequest
    {
        public int Id { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin, "test.softdelete"];
        public bool BypassCache => false;
        public string? CacheKey => $"GetByIdTest-{Id}";
        public string CacheGroupKey => "GetTests";
        public int Interval => 1000;

        public class SoftDeleteTestCommandHandler : IRequestHandler<SoftDeleteTestCommand, SoftDeleteTestResponseDTO>
        {
            private readonly ITestRepository _testRepository;
            private readonly IMapper _mapper;
            private readonly TestBusinessRules _testBusinessRules;

            public SoftDeleteTestCommandHandler(
                ITestRepository testRepository,
                IMapper mapper,
                TestBusinessRules testBusinessRules)
            {
                _testRepository = testRepository;
                _mapper = mapper;
                _testBusinessRules = testBusinessRules;
            }

            public async Task<SoftDeleteTestResponseDTO> Handle(SoftDeleteTestCommand request, CancellationToken cancellationToken)
            {
                // Business rules validation
                await _testBusinessRules.TestShouldExistWhenSelected(request.Id);

                // Get existing test
                Test? test = await _testRepository.GetAsync(
                    predicate: t => t.Id == request.Id && !t.IsDeleted,
                    enableTracking: true,
                    cancellationToken: cancellationToken
                );

                _testBusinessRules.TestShouldNotBeDeleted(test!);

                // Soft delete
                await _testRepository.SoftDeleteAsync(test!);

                SoftDeleteTestResponseDTO response = _mapper.Map<SoftDeleteTestResponseDTO>(test);
                return response;
            }
        }
    }
}