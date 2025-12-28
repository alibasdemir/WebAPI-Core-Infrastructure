using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails
{
    public class ValidationProblemDetails : ProblemDetails
    {
        public IEnumerable<ValidationExceptionModel> Errors { get; init; }
        public ValidationProblemDetails(IEnumerable<ValidationExceptionModel> errors)
        {
            Errors = errors;
            Title = "Validation error(s)";
            Detail = "One or more validation errors occurred.";
            Type = "ValidationException";
            Status = StatusCodes.Status400BadRequest;
        }
    }
}
