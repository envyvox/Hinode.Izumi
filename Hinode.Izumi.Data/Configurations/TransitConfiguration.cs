using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class TransitConfiguration : EntityTypeConfigurationBase<Transit>
    {
        public override void Configure(EntityTypeBuilder<Transit> b)
        {
            b.HasIndex(x => new {x.Departure, x.Destination}).IsUnique();

            b.Property(x => x.Time).IsRequired();
            b.Property(x => x.Price).IsRequired();

            base.Configure(b);
        }
    }
}
