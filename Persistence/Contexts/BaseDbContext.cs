using Core.Security.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all entity configurations from the current assembly (including seed data)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Fluent API configurations for User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                // Email verification properties
                entity.Property(u => u.IsEmailVerified)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(u => u.EmailVerificationToken)
                    .HasMaxLength(500);

                entity.Property(u => u.EmailVerificationTokenExpires);

                // Password reset properties
                entity.Property(u => u.PasswordResetToken)
                    .HasMaxLength(500);

                entity.Property(u => u.PasswordResetTokenExpires);

                // Security tracking properties
                entity.Property(u => u.LastLoginDate);

                entity.Property(u => u.LastLoginIp)
                    .HasMaxLength(45); // IPv6 max length

                // Unique indexes to prevent duplicate email and username
                entity.HasIndex(u => u.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Email");

                entity.HasIndex(u => u.UserName)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Username");

                // Index for token lookups (performance optimization)
                entity.HasIndex(u => u.EmailVerificationToken)
                    .HasDatabaseName("IX_Users_EmailVerificationToken");

                entity.HasIndex(u => u.PasswordResetToken)
                    .HasDatabaseName("IX_Users_PasswordResetToken");
            });

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
