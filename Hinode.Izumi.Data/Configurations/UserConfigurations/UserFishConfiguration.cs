using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserFishConfiguration : EntityTypeConfigurationBase<UserFish>
    {
        public override void Configure(EntityTypeBuilder<UserFish> b)
        {
            b.HasIndex(x => new {x.UserId, x.FishId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Fish)
                .WithMany()
                .HasForeignKey(x => x.FishId);

            base.Configure(b);
        }
    }
}
