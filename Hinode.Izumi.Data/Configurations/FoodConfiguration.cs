using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class FoodConfiguration : EntityTypeConfigurationBase<Food>
    {
        public override void Configure(EntityTypeBuilder<Food> b)
        {
            b.HasIndex(x => new {x.Name, x.Event}).IsUnique();

            b.Property(x => x.Mastery).IsRequired();
            b.Property(x => x.Time).IsRequired();
            b.Property(x => x.RecipeSellable).IsRequired().HasDefaultValue(false);
            b.Property(x => x.Event).IsRequired().HasDefaultValue(false);

            base.Configure(b);
        }
    }
}
