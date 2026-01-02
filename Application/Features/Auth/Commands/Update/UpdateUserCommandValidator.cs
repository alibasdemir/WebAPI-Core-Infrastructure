using FluentValidation;

namespace Application.Features.Auth.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User ID is required.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("First name can only contain letters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("Last name can only contain letters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.")
                .Matches(@"^[a-zA-Z0-9._]+$").WithMessage("Username can only contain letters, numbers, dots, and underscores.");
        }
    }
}