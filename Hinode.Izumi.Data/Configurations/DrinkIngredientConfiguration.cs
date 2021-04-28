using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class DrinkIngredientConfiguration : EntityTypeConfigurationBase<DrinkIngredient>
    {
        public override void Configure(EntityTypeBuilder<DrinkIngredient> b)
        {
            b.HasIndex(x => new {x.DrinkId, x.Category, x.IngredientId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.Drink)
                .WithMany()
                .HasForeignKey(x => x.DrinkId);

            base.Configure(b);
        }
    }
}
