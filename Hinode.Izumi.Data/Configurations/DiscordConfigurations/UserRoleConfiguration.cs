using Hinode.Izumi.Data.Models.DiscordModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.DiscordConfigurations
{
    public class UserRoleConfiguration : EntityTypeConfigurationBase<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> b)
        {
            b.HasIndex(x => new {x.UserId, x.RoleId}).IsUnique();

            b.Property(x => x.Expiration).IsRequired();

            base.Configure(b);
        }
    }
}
