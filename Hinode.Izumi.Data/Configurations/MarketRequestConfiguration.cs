using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class MarketRequestConfiguration : EntityTypeConfigurationBase<MarketRequest>
    {
        public override void Configure(EntityTypeBuilder<MarketRequest> b)
        {
            b.HasIndex(x => new {x.UserId, x.Category, x.ItemId}).IsUnique();

            b.Property(x => x.Price).IsRequired();
            b.Property(x => x.Amount).IsRequired();
            b.Property(x => x.Selling).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
