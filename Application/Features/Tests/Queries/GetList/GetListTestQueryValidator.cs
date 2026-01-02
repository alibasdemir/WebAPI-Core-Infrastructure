using FluentValidation;

namespace Application.Features.Tests.Queries.GetList
{
    public class GetListTestQueryValidator : AbstractValidator<GetListTestQuery>
    {
        public GetListTestQueryValidator()
        {
            RuleFor(x => x.PageRequest.Index)
                .GreaterThanOrEqualTo(0).WithMessage("Page index must be 0 or greater.");

            RuleFor(x => x.PageRequest.Size)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");

            // Dynamic query validation
            RuleFor(x => x.Dynamic)
                .Must(d => d == null || (d.Sort != null && d.Sort.Any()) || (d.Filter != null))
                .WithMessage("Dynamic query must contain at least Sort or Filter when provided.");
        }
    }
}