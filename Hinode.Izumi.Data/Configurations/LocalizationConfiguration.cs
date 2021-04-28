using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class LocalizationConfiguration : EntityTypeConfigurationBase<Localization>
    {
        public override void Configure(EntityTypeBuilder<Localization> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Category).IsRequired();
            b.Property(x => x.ItemId).IsRequired();
            b.Property(x => x.Single).IsRequired();
            b.Property(x => x.Double).IsRequired();
            b.Property(x => x.Multiply).IsRequired();

            base.Configure(b);
        }
    }
}
