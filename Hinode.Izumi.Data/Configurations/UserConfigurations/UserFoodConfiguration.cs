using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserFoodConfiguration : EntityTypeConfigurationBase<UserFood>
    {
        public override void Configure(EntityTypeBuilder<UserFood> b)
        {
            b.HasIndex(x => new {x.UserId, x.FoodId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Food)
                .WithMany()
                .HasForeignKey(x => x.FoodId);

            base.Configure(b);
        }
    }
}
