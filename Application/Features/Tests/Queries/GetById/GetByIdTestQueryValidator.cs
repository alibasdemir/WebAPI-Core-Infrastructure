using FluentValidation;

namespace Application.Features.Tests.Queries.GetById
{
    public class GetByIdTestQueryValidator : AbstractValidator<GetByIdTestQuery>
    {
        public GetByIdTestQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Test ID must be greater than 0.");
        }
    }
}