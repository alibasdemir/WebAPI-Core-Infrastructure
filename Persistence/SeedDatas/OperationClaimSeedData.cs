using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.SeedDatas
{
    public class OperationClaimSeedData : IEntityTypeConfiguration<OperationClaim>
    {
        public void Configure(EntityTypeBuilder<OperationClaim> builder)
        {
            OperationClaim[] operationClaimSeeds =
           {
                new OperationClaim
                {
                    Id = 1,
                    Name = "admin",
                },
                new OperationClaim
                {
                    Id = 2,
                    Name = "test.create",
                },
                new OperationClaim
                {
                    Id = 3,
                    Name = "test.read",
                },
                new OperationClaim
                {
                    Id = 4,
                    Name = "test.update",
                },
                new OperationClaim
                {
                    Id = 5,
                    Name = "test.delete",
                },
            };

            builder.HasData(operationClaimSeeds);
        }
    }
}