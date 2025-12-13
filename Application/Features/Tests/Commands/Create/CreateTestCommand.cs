using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Commands.Create
{
    public class CreateTestCommand : IRequest<CreateTestResponseDTO>
    {
        public string Name { get; set; }

        public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, CreateTestResponseDTO>
        {
            private readonly ITestRepository _testRepository;

            public CreateTestCommandHandler(ITestRepository testRepository)
            {
                _testRepository = testRepository;
            }

            public async Task<CreateTestResponseDTO> Handle(CreateTestCommand request, CancellationToken cancellationToken)
            {
                Test test = new()
                {
                    Name = request.Name,
                    CreatedDate = DateTime.UtcNow
                };
                await _testRepository.AddAsync(test);

                return new CreateTestResponseDTO
                {
                    Id = test.Id,
                    Name = test.Name
                };
            }
        }
    }
}
