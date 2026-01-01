using Core.DataAccess;
using Core.Security.Entities;

namespace Application.Repositories
{
    public interface IUserOperationClaimRepository : IAsyncRepository<UserOperationClaim>, IRepository<UserOperationClaim>
    {
    }
}
