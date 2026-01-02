using FluentValidation;

namespace Application.Features.UserOperationClaims.Queries.GetList
{
    public class GetListUserOperationClaimQueryValidator : AbstractValidator<GetListUserOperationClaimQuery>
    {
        public GetListUserOperationClaimQueryValidator()
        {
            RuleFor(x => x.PageRequest.Index)
                .GreaterThanOrEqualTo(0).WithMessage("Page index must be 0 or greater.");

            RuleFor(x => x.PageRequest.Size)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
        }
    }
}