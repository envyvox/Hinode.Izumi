using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class WeeklyQuestConfiguration : EntityTypeConfigurationBase<WeeklyQuest>
    {
        public override void Configure(EntityTypeBuilder<WeeklyQuest> b)
        {
            b.Property(x => x.Season).IsRequired();
            b.Property(x => x.Category).IsRequired();
            b.Property(x => x.Difficulty).IsRequired();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.ItemCategory).IsRequired();
            b.Property(x => x.ItemId).IsRequired();
            b.Property(x => x.ItemAmount).IsRequired();

            base.Configure(b);
        }
    }
}
