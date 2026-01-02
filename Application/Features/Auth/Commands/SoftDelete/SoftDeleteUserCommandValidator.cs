using FluentValidation;

namespace Application.Features.Auth.Commands.SoftDeleteUser
{
    public class SoftDeleteUserCommandValidator : AbstractValidator<SoftDeleteUserCommand>
    {
        public SoftDeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User ID is required.");
        }
    }
}