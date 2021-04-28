using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class CardConfiguration : EntityTypeConfigurationBase<Card>
    {
        public override void Configure(EntityTypeBuilder<Card> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Rarity).IsRequired();
            b.Property(x => x.Effect).IsRequired();
            b.Property(x => x.Anime).IsRequired();
            b.Property(x => x.Url).IsRequired();

            base.Configure(b);
        }
    }
}
