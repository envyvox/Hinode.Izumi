using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserReferrerConfiguration : EntityTypeConfigurationBase<UserReferrer>
    {
        public override void Configure(EntityTypeBuilder<UserReferrer> b)
        {
            b.HasIndex(x => new {x.UserId, x.ReferrerId}).IsUnique();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Referrer)
                .WithMany()
                .HasForeignKey(x => x.ReferrerId);

            base.Configure(b);
        }
    }
}
