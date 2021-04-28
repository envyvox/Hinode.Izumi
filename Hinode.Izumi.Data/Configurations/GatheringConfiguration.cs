using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class GatheringConfiguration : EntityTypeConfigurationBase<Gathering>
    {
        public override void Configure(EntityTypeBuilder<Gathering> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Price).IsRequired();
            b.Property(x => x.Location).IsRequired();

            base.Configure(b);
        }
    }
}
