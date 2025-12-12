using Core.DataAccess;
using Domain.Entities;

namespace Application.Repositories
{
    public interface ITestRepository : IAsyncRepository<Test>, IRepository<Test>
    {
    }
}
