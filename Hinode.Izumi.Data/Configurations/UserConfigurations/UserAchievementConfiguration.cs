using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserAchievementConfiguration : EntityTypeConfigurationBase<UserAchievement>
    {
        public override void Configure(EntityTypeBuilder<UserAchievement> b)
        {
            b.HasIndex(x => new {x.UserId, x.AchievementId}).IsUnique();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Achievement)
                .WithMany()
                .HasForeignKey(x => x.AchievementId);

            base.Configure(b);
        }
    }
}
