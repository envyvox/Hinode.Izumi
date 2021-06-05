using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class CurrentWeeklyQuestConfiguration : EntityTypeConfigurationBase<CurrentWeeklyQuest>
    {
        public override void Configure(EntityTypeBuilder<CurrentWeeklyQuest> b)
        {
            b.HasIndex(x => new {x.Location, x.QuestId}).IsUnique();

            b
                .HasOne(x => x.Quest)
                .WithMany()
                .HasForeignKey(x => x.QuestId);

            base.Configure(b);
        }
    }
}
