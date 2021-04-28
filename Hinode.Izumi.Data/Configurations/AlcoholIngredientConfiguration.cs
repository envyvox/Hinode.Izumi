using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class AlcoholIngredientConfiguration : EntityTypeConfigurationBase<AlcoholIngredient>
    {
        public override void Configure(EntityTypeBuilder<AlcoholIngredient> b)
        {
            b.HasIndex(x => new {x.AlcoholId, x.Category, x.IngredientId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.Alcohol)
                .WithMany()
                .HasForeignKey(x => x.AlcoholId);

            base.Configure(b);
        }
    }
}
