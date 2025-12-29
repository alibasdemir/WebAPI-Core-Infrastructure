using Core.Security.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=test;Trusted_Connection=True;MultipleActiveResultSets=true"); // Connection string should be provided here.
            base.OnConfiguring(optionsBuilder);
        }
    }
}
