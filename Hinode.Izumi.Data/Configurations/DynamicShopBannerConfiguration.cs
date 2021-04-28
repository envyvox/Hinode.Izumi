using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class DynamicShopBannerConfiguration : EntityTypeConfigurationBase<DynamicShopBanner>
    {
        public override void Configure(EntityTypeBuilder<DynamicShopBanner> b)
        {
            b.HasIndex(x => x.BannerId).IsUnique();

            b
                .HasOne(x => x.Banner)
                .WithMany()
                .HasForeignKey(x => x.BannerId);

            base.Configure(b);
        }
    }
}
