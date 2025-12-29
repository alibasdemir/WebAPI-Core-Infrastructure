using Core.DataAccess;
using Core.Security.Entities;

namespace Application.Repositories
{
    public interface IUserRepository : IAsyncRepository<User>, IRepository<User>
    {
    }
}
