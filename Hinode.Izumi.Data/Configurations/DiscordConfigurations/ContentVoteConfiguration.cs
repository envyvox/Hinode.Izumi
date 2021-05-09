using Hinode.Izumi.Data.Models.DiscordModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.DiscordConfigurations
{
    public class ContentVoteConfiguration : EntityTypeConfigurationBase<ContentVote>
    {
        public override void Configure(EntityTypeBuilder<ContentVote> b)
        {
            b.HasIndex(x => new {x.UserId, x.MessageId, x.Vote}).IsUnique();

            b.Property(x => x.Active).IsRequired();

            b
                .HasOne(x => x.Message)
                .WithMany()
                .HasForeignKey(x => x.MessageId);

            base.Configure(b);
        }
    }
}
