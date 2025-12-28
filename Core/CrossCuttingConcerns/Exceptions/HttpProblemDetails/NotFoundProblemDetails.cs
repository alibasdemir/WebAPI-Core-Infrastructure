using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails
{
    public class NotFoundProblemDetails : ProblemDetails
    {
        public NotFoundProblemDetails(string detail)
        {
            Title = "Not found";
            Detail = detail;
            Type = "NotFoundError";
            Status = StatusCodes.Status404NotFound;
        }
    }
}
