using Core.Security.Entities;

namespace Core.Security.JWT
{
    public interface IJwtHelper
    {
        AccessToken CreateToken(User user, IList<OperationClaim> operationClaims);
    }
}
