using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserWeeklyQuestConfiguration : EntityTypeConfigurationBase<UserWeeklyQuest>
    {
        public override void Configure(EntityTypeBuilder<UserWeeklyQuest> b)
        {
            b.HasIndex(x => new {x.UserId, x.QuestId}).IsUnique();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Quest)
                .WithMany()
                .HasForeignKey(x => x.QuestId);

            base.Configure(b);
        }
    }
}
