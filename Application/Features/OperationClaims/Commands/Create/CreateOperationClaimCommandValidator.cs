using FluentValidation;

namespace Application.Features.OperationClaims.Commands.Create
{
    public class CreateOperationClaimCommandValidator : AbstractValidator<CreateOperationClaimCommand>
    {
        public CreateOperationClaimCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Operation claim name is required.")
                .MinimumLength(2).WithMessage("Operation claim name must be at least 2 characters.")
                .MaximumLength(50).WithMessage("Operation claim name cannot exceed 50 characters.")
                .Matches(@"^[a-z0-9._-]+$").WithMessage("Operation claim name can only contain lowercase letters, numbers, dots, underscores, and hyphens.");
        }
    }
}