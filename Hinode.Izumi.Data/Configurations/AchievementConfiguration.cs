using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class AchievementConfiguration : EntityTypeConfigurationBase<Achievement>
    {
        public override void Configure(EntityTypeBuilder<Achievement> b)
        {
            b.HasIndex(x => x.Type).IsUnique();

            b.Property(x => x.Category).IsRequired();
            b.Property(x => x.Reward).IsRequired();
            b.Property(x => x.Number).IsRequired();

            base.Configure(b);
        }
    }
}
