using Hinode.Izumi.Data.Models.DiscordModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.DiscordConfigurations
{
    public class ContentMessageConfiguration : EntityTypeConfigurationBase<ContentMessage>
    {
        public override void Configure(EntityTypeBuilder<ContentMessage> b)
        {
            b.HasIndex(x => new {x.UserId, x.ChannelId, x.MessageId}).IsUnique();

            base.Configure(b);
        }
    }
}
