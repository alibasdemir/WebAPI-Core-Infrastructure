using FluentValidation;

namespace Application.Features.Tests.Commands.Create
{
    public class CreateTestCommandValidator : AbstractValidator<CreateTestCommand>
    {
        public CreateTestCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
                .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");
        }
    }
}
