using Core.DataAccess;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class TestRepository : EfRepositoryBase<Test, BaseDbContext>
    {
        public TestRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
