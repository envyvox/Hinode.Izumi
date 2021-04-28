using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserBuildingConfiguration : EntityTypeConfigurationBase<UserBuilding>
    {
        public override void Configure(EntityTypeBuilder<UserBuilding> b)
        {
            b.HasIndex(x => new {x.UserId, x.BuildingId}).IsUnique();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Building)
                .WithMany()
                .HasForeignKey(x => x.BuildingId);

            base.Configure(b);
        }
    }
}
