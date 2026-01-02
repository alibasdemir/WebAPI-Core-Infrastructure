using FluentValidation;

namespace Application.Features.Tests.Commands.Delete
{
    public class DeleteTestCommandValidator : AbstractValidator<DeleteTestCommand>
    {
        public DeleteTestCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Test ID must be greater than 0.");
        }
    }
}