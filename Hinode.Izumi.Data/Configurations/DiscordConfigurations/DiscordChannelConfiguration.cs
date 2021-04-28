using Hinode.Izumi.Data.Models.DiscordModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.DiscordConfigurations
{
    public class DiscordChannelConfiguration : EntityTypeConfigurationBase<DiscordChannel>
    {
        public override void Configure(EntityTypeBuilder<DiscordChannel> b)
        {
            b.HasIndex(x => x.Channel).IsUnique();

            b.Property(x => x.Id).ValueGeneratedNever();

            base.Configure(b);
        }
    }
}
