using Hinode.Izumi.Data.Models.FamilyModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.FamilyConfigurations
{
    public class FamilyBuildingConfiguration : EntityTypeConfigurationBase<FamilyBuilding>
    {
        public override void Configure(EntityTypeBuilder<FamilyBuilding> b)
        {
            b.HasIndex(x => new {x.FamilyId, x.BuildingId}).IsUnique();

            b
                .HasOne(x => x.Family)
                .WithMany()
                .HasForeignKey(x => x.FamilyId);

            b
                .HasOne(x => x.Building)
                .WithMany()
                .HasForeignKey(x => x.BuildingId);

            base.Configure(b);
        }
    }
}
