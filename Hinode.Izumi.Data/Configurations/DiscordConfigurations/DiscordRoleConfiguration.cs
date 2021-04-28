using Hinode.Izumi.Data.Models.DiscordModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.DiscordConfigurations
{
    public class DiscordRoleConfiguration : EntityTypeConfigurationBase<DiscordRole>
    {
        public override void Configure(EntityTypeBuilder<DiscordRole> b)
        {
            b.HasIndex(x => x.Role).IsUnique();

            b.Property(x => x.Id).ValueGeneratedNever();

            base.Configure(b);
        }
    }
}
