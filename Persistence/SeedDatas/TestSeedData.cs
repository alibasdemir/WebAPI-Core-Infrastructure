using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.SeedDatas
{
    public class TestSeedData : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            Test[] testSeeds =
            {
                new Test
                {
                    Id = 1,
                    Name = "test1"
                },
                new Test
                {
                    Id = 2,
                    Name = "test2"
                },
                new Test
                {
                    Id = 3,
                    Name = "test3"
                },
            };

            builder.HasData(testSeeds);
        }
    }
}
