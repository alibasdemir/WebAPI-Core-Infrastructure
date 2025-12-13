using Application.Repositories;
using MediatR;

namespace Application.Features.Test.Commands.Create
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
                throw new NotImplementedException();
            }
        }
    }
}
