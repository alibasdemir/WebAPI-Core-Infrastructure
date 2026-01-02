using FluentValidation;

namespace Application.Features.OperationClaims.Queries.GetById
{
    public class GetByIdOperationClaimQueryValidator : AbstractValidator<GetByIdOperationClaimQuery>
    {
        public GetByIdOperationClaimQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Operation claim ID must be greater than 0.");
        }
    }
}