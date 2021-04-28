using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class DrinkConfiguration : EntityTypeConfigurationBase<Drink>
    {
        public override void Configure(EntityTypeBuilder<Drink> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Time).IsRequired();

            base.Configure(b);
        }
    }
}
