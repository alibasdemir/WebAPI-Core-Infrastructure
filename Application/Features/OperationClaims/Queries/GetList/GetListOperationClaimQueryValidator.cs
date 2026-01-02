using FluentValidation;

namespace Application.Features.OperationClaims.Queries.GetList
{
    public class GetListOperationClaimQueryValidator : AbstractValidator<GetListOperationClaimQuery>
    {
        public GetListOperationClaimQueryValidator()
        {
            RuleFor(x => x.Index)
                .GreaterThanOrEqualTo(0).WithMessage("Page index must be 0 or greater.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
        }
    }
}