using FluentValidation;

namespace Application.Features.UserOperationClaims.Queries.GetByUserId
{
    public class GetUserOperationClaimsByUserIdQueryValidator : AbstractValidator<GetUserOperationClaimsByUserIdQuery>
    {
        public GetUserOperationClaimsByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");
        }
    }
}