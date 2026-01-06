using Application.Features.Tests.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Core.Application.Pipelines.Logging;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Commands.Update
{
    public class UpdateTestCommand : IRequest<UpdateTestResponseDTO>, ISecuredRequest, ILoggableRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin, "test.update"];

        public class UpdateTestCommandHandler : IRequestHandler<UpdateTestCommand, UpdateTestResponseDTO>
        {
            private readonly ITestRepository _testRepository;
            private readonly IMapper _mapper;
            private readonly TestBusinessRules _testBusinessRules;

            public UpdateTestCommandHandler(
                ITestRepository testRepository,
                IMapper mapper,
                TestBusinessRules testBusinessRules)
            {
                _testRepository = testRepository;
                _mapper = mapper;
                _testBusinessRules = testBusinessRules;
            }

            public async Task<UpdateTestResponseDTO> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
            {
                // Business rules validation
                await _testBusinessRules.TestShouldExistWhenSelected(request.Id);
                _testBusinessRules.TestNameShouldBeValid(request.Name);
                await _testBusinessRules.TestNameShouldBeUniqueForUpdate(request.Name, request.Id);

                // Get existing test
                Test? test = await _testRepository.GetAsync(
                    predicate: t => t.Id == request.Id,
                    enableTracking: true,
                    cancellationToken: cancellationToken
                );

                _testBusinessRules.TestShouldNotBeDeleted(test!);

                // Update properties
                test!.Name = request.Name;
                test.UpdatedDate = DateTime.UtcNow;

                await _testRepository.UpdateAsync(test);

                UpdateTestResponseDTO response = _mapper.Map<UpdateTestResponseDTO>(test);
                return response;
            }
        }
    }
}