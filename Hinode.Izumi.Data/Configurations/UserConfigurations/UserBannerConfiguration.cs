using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserBannerConfiguration : EntityTypeConfigurationBase<UserBanner>
    {
        public override void Configure(EntityTypeBuilder<UserBanner> b)
        {
            b.HasIndex(x => new {x.UserId, x.BannerId}).IsUnique();

            b.Property(x => x.Active).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Banner)
                .WithMany()
                .HasForeignKey(x => x.BannerId);

            base.Configure(b);
        }
    }
}
