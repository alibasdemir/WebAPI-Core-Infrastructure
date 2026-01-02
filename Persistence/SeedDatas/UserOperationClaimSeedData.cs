using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.SeedDatas
{
    public class UserOperationClaimSeedData : IEntityTypeConfiguration<UserOperationClaim>
    {
        public void Configure(EntityTypeBuilder<UserOperationClaim> builder)
        {
            UserOperationClaim[] userOperationClaimSeeds =
           {
                new UserOperationClaim
                {
                    Id = 1,
                    UserId = 1,
                    OperationClaimId = 1,
                },
                new UserOperationClaim
                {
                    Id = 2,
                    UserId = 2,
                    OperationClaimId = 3,
                },
            };

            builder.HasData(userOperationClaimSeeds);
        }
    }
}
