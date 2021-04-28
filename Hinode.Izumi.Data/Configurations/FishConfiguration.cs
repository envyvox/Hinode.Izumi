using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class FishConfiguration : EntityTypeConfigurationBase<Fish>
    {
        public override void Configure(EntityTypeBuilder<Fish> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Rarity).IsRequired();
            b.Property(x => x.Seasons).IsRequired();
            b.Property(x => x.Weather).IsRequired();
            b.Property(x => x.TimesDay).IsRequired();
            b.Property(x => x.Price).IsRequired();

            base.Configure(b);
        }
    }
}
