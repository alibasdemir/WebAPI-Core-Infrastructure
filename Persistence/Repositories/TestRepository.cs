using Application.Repositories;
using Core.DataAccess;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class TestRepository : EfRepositoryBase<Test, BaseDbContext>, ITestRepository
    {
        public TestRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
