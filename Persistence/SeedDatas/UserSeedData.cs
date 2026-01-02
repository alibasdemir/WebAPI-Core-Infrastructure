using Core.Security.Entities;
using Core.Security.HashingSalting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.SeedDatas
{
    public class UserSeedData : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            string password = "admin";
            byte[] passwordHash, passwordSalt;
            HashingSaltingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            string password2 = "test";
            byte[] passwordHash2, passwordSalt2;
            HashingSaltingHelper.CreatePasswordHash(password2, out passwordHash2, out passwordSalt2);

            User[] userSeeds =
            {
                new User
                {
                    Id = 1,
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin",
                    UserName = "admin",
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                },
                new User
                {
                    Id = 2,
                    FirstName = "test",
                    LastName = "test",
                    Email = "test",
                    UserName = "test",
                    PasswordSalt = passwordSalt2,
                    PasswordHash = passwordHash2,
                },
            };

            builder.HasData(userSeeds);
        }
    }
}
