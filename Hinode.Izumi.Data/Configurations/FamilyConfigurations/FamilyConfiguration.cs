using Hinode.Izumi.Data.Models.FamilyModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.FamilyConfigurations
{
    public class FamilyConfiguration : EntityTypeConfigurationBase<Family>
    {
        public override void Configure(EntityTypeBuilder<Family> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.Description);

            base.Configure(b);
        }
    }
}
