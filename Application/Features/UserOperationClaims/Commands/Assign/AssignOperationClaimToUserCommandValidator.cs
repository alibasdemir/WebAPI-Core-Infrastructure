using FluentValidation;

namespace Application.Features.UserOperationClaims.Commands.Assign
{
    public class AssignOperationClaimToUserCommandValidator : AbstractValidator<AssignOperationClaimToUserCommand>
    {
        public AssignOperationClaimToUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");

            RuleFor(x => x.OperationClaimId)
                .GreaterThan(0).WithMessage("Operation claim ID must be greater than 0.");
        }
    }
}