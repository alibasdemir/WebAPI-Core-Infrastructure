using FluentValidation;
using FluentValidation.Results;

namespace Core.Application.Pipelines.Validation
{
    // For manual use WITHOUT MediatR (old method)
    public class ValidationTool
    {
        public static void Validate(IValidator validator, object entity)
        {
            ValidationContext<object> context = new(entity);
            ValidationResult result = validator.Validate(context);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }
    }

    // Usage Example:
    //public class SomeService
    //{
    //    public void CreateUser(CreateUserRequest request)
    //    {
    //        // Manuel Validation
    //        ValidationTool.Validate(new CreateUserValidator(), request);

    //        // Business Logic...
    //    }
    //}
}
