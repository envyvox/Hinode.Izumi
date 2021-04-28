using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserCooldownConfiguration : EntityTypeConfigurationBase<UserCooldown>
    {
        public override void Configure(EntityTypeBuilder<UserCooldown> b)
        {
            b.HasIndex(x => new {x.UserId, x.Cooldown}).IsUnique();

            b.Property(x => x.Expiration).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
