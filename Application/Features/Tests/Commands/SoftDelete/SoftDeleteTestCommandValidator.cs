using FluentValidation;

namespace Application.Features.Tests.Commands.SoftDelete
{
    public class SoftDeleteTestCommandValidator : AbstractValidator<SoftDeleteTestCommand>
    {
        public SoftDeleteTestCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Test ID must be greater than 0.");
        }
    }
}