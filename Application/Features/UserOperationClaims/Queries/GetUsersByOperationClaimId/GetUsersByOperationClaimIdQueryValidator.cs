using FluentValidation;

namespace Application.Features.UserOperationClaims.Queries.GetUsersByOperationClaimId
{
    public class GetUsersByOperationClaimIdQueryValidator : AbstractValidator<GetUsersByOperationClaimIdQuery>
    {
        public GetUsersByOperationClaimIdQueryValidator()
        {
            RuleFor(x => x.OperationClaimId)
                .GreaterThan(0).WithMessage("Operation claim ID must be greater than 0.");
        }
    }
}