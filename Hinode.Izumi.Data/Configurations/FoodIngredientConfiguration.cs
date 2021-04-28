using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class FoodIngredientConfiguration : EntityTypeConfigurationBase<FoodIngredient>
    {
        public override void Configure(EntityTypeBuilder<FoodIngredient> b)
        {
            b.HasIndex(x => new {x.FoodId, x.Category, x.IngredientId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.Food)
                .WithMany()
                .HasForeignKey(x => x.FoodId);

            base.Configure(b);
        }
    }
}
