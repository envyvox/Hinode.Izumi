using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class SeedConfiguration : EntityTypeConfigurationBase<Seed>
    {
        public override void Configure(EntityTypeBuilder<Seed> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Season).IsRequired();
            b.Property(x => x.Growth).IsRequired();
            b.Property(x => x.ReGrowth).IsRequired();
            b.Property(x => x.Price).IsRequired();
            b.Property(x => x.Multiply).IsRequired();

            base.Configure(b);
        }
    }
}
