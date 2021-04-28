using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class WorldPropertyConfiguration : EntityTypeConfigurationBase<WorldProperty>
    {
        public override void Configure(EntityTypeBuilder<WorldProperty> b)
        {
            b.HasIndex(x => x.Property).IsUnique();

            b.Property(x => x.PropertyCategory).IsRequired();
            b.Property(x => x.Value).IsRequired();

            base.Configure(b);
        }
    }
}
