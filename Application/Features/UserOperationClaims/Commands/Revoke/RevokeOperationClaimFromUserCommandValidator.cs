using FluentValidation;

namespace Application.Features.UserOperationClaims.Commands.Revoke
{
    public class RevokeOperationClaimFromUserCommandValidator : AbstractValidator<RevokeOperationClaimFromUserCommand>
    {
        public RevokeOperationClaimFromUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User operation claim ID must be greater than 0.");
        }
    }
}