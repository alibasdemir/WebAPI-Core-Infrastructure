using FluentValidation;

namespace Application.Features.UserOperationClaims.Commands.SoftDelete
{
    public class SoftDeleteUserOperationClaimCommandValidator : AbstractValidator<SoftDeleteUserOperationClaimCommand>
    {
        public SoftDeleteUserOperationClaimCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User operation claim ID must be greater than 0.");
        }
    }
}