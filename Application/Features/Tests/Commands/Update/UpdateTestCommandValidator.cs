using FluentValidation;

namespace Application.Features.Tests.Commands.Update
{
    public class UpdateTestCommandValidator : AbstractValidator<UpdateTestCommand>
    {
        public UpdateTestCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Test ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}