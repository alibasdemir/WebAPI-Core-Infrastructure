using Application.Features.Auth.Queries.GetById;
using FluentValidation;

namespace Application.Features.Auth.Queries.GetByIdUser
{
    public class GetByIdUserQueryValidator : AbstractValidator<GetByIdUserQuery>
    {
        public GetByIdUserQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");
        }
    }
}