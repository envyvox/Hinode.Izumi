using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class ProductConfiguration : EntityTypeConfigurationBase<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Price).IsRequired();

            base.Configure(b);
        }
    }
}
