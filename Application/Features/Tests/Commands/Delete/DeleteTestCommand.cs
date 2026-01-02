using Application.Features.Tests.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Authorization.Constants;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Commands.Delete
{
    public class DeleteTestCommand : IRequest<DeleteTestResponseDTO>, ISecuredRequest
    {
        public int Id { get; set; }
        public string[] Roles => [GeneralOperationClaims.Admin];

        public class DeleteTestCommandHandler : IRequestHandler<DeleteTestCommand, DeleteTestResponseDTO>
        {
            private readonly ITestRepository _testRepository;
            private readonly IMapper _mapper;
            private readonly TestBusinessRules _testBusinessRules;

            public DeleteTestCommandHandler(
                ITestRepository testRepository,
                IMapper mapper,
                TestBusinessRules testBusinessRules)
            {
                _testRepository = testRepository;
                _mapper = mapper;
                _testBusinessRules = testBusinessRules;
            }

            public async Task<DeleteTestResponseDTO> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
            {
                // Business rules validation
                await _testBusinessRules.TestShouldExistWhenSelected(request.Id);

                // Get existing test
                Test? test = await _testRepository.GetAsync(
                    predicate: t => t.Id == request.Id,
                    enableTracking: true,
                    cancellationToken: cancellationToken
                );

                // Hard delete
                await _testRepository.DeleteAsync(test!);

                DeleteTestResponseDTO response = _mapper.Map<DeleteTestResponseDTO>(test);
                return response;
            }
        }
    }
}