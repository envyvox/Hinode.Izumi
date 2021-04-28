using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserRecipeConfiguration : EntityTypeConfigurationBase<UserRecipe>
    {
        public override void Configure(EntityTypeBuilder<UserRecipe> b)
        {
            b.HasIndex(x => new {x.UserId, x.FoodId}).IsUnique();

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
