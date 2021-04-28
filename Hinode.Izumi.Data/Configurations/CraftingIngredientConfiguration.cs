using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class CraftingIngredientConfiguration : EntityTypeConfigurationBase<CraftingIngredient>
    {
        public override void Configure(EntityTypeBuilder<CraftingIngredient> b)
        {
            b.HasIndex(x => new {x.CraftingId, x.Category, x.IngredientId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.Crafting)
                .WithMany()
                .HasForeignKey(x => x.CraftingId);

            base.Configure(b);
        }
    }
}
