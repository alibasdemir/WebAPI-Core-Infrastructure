using FluentValidation;

namespace Application.Features.Auth.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User ID is required.");
        }
    }
}