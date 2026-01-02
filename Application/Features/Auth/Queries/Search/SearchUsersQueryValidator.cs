using FluentValidation;

namespace Application.Features.Auth.Queries.SearchUsers
{
    public class SearchUsersQueryValidator : AbstractValidator<SearchUsersQuery>
    {
        public SearchUsersQueryValidator()
        {
            RuleFor(x => x.SearchTerm)
                .MaximumLength(100).WithMessage("Search term cannot exceed 100 characters.");

            RuleFor(x => x.Index)
                .GreaterThanOrEqualTo(0).WithMessage("Page index must be 0 or greater.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
        }
    }
}