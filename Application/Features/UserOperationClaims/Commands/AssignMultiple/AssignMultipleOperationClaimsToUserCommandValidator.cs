using FluentValidation;

namespace Application.Features.UserOperationClaims.Commands.AssignMultiple
{
    public class AssignMultipleOperationClaimsToUserCommandValidator : AbstractValidator<AssignMultipleOperationClaimsToUserCommand>
    {
        public AssignMultipleOperationClaimsToUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");

            RuleFor(x => x.OperationClaimIds)
                .NotEmpty().WithMessage("Operation claim IDs cannot be empty.")
                .Must(ids => ids != null && ids.Count > 0).WithMessage("At least one operation claim ID is required.");
        }
    }
}