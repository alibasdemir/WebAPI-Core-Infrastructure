using Core.DataAccess;
using Core.Security.Entities;

namespace Application.Repositories
{
    public interface IOperationClaimRepository : IAsyncRepository<OperationClaim>, IRepository<OperationClaim>
    {
    }
}
