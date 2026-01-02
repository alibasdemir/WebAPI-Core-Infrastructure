using FluentValidation;

namespace Application.Features.OperationClaims.Commands.Update
{
    public class UpdateOperationClaimCommandValidator : AbstractValidator<UpdateOperationClaimCommand>
    {
        public UpdateOperationClaimCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Operation claim ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Operation claim name is required.")
                .MinimumLength(2).WithMessage("Operation claim name must be at least 2 characters.")
                .MaximumLength(50).WithMessage("Operation claim name cannot exceed 50 characters.")
                .Matches(@"^[a-z0-9._-]+$").WithMessage("Operation claim name can only contain lowercase letters, numbers, dots, underscores, and hyphens.");
        }
    }
}