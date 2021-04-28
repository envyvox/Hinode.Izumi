using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class CraftingConfiguration : EntityTypeConfigurationBase<Crafting>
    {
        public override void Configure(EntityTypeBuilder<Crafting> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Time).IsRequired();
            b.Property(x => x.Location).IsRequired();

            base.Configure(b);
        }
    }
}
