using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class BuildingConfiguration : EntityTypeConfigurationBase<Building>
    {
        public override void Configure(EntityTypeBuilder<Building> b)
        {
            b.HasIndex(x => x.Type).IsUnique();

            b.Property(x => x.ProjectId).HasDefaultValue(null);
            b.Property(x => x.Category).IsRequired();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Description).IsRequired();

            b
                .HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId);

            base.Configure(b);
        }
    }
}
