using Core.Security.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=test;Trusted_Connection=True;MultipleActiveResultSets=true"); // Connection string should be provided here.
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");

            // Fluent API configurations for OperationClaim
            modelBuilder.Entity<OperationClaim>(entity =>
            {
                entity.ToTable("OperationClaims");
                entity.HasKey(oc => oc.Id);
                
                entity.Property(oc => oc.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(oc => oc.Name)
                    .IsUnique();
            });

            // Fluent API configurations for UserOperationClaim
            modelBuilder.Entity<UserOperationClaim>(entity =>
            {
                entity.ToTable("UserOperationClaims");
                entity.HasKey(uoc => uoc.Id);

                entity.Property(uoc => uoc.UserId)
                    .IsRequired();

                entity.Property(uoc => uoc.OperationClaimId)
                    .IsRequired();

                // Foreign key relationship with User
                entity.HasOne(uoc => uoc.User)
                    .WithMany()
                    .HasForeignKey(uoc => uoc.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Foreign key relationship with OperationClaim
                entity.HasOne(uoc => uoc.OperationClaim)
                    .WithMany()
                    .HasForeignKey(uoc => uoc.OperationClaimId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Composite unique index to prevent duplicate user-claim assignments
                entity.HasIndex(uoc => new { uoc.UserId, uoc.OperationClaimId })
                    .IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
