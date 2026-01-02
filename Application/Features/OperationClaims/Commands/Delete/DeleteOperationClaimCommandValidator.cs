using FluentValidation;

namespace Application.Features.OperationClaims.Commands.Delete
{
    public class DeleteOperationClaimCommandValidator : AbstractValidator<DeleteOperationClaimCommand>
    {
        public DeleteOperationClaimCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Operation claim ID must be greater than 0.");
        }
    }
}