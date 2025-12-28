using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails
{
    public class AuthorizationProblemDetails : ProblemDetails
    {
        public AuthorizationProblemDetails(string detail)
        {
            Title = "Authorization error";
            Detail = detail;
            Type = "AuthorizationException";
            Status = StatusCodes.Status401Unauthorized;
        }
    }
}
