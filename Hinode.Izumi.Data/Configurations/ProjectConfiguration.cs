using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class ProjectConfiguration : EntityTypeConfigurationBase<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Price).IsRequired();
            b.Property(x => x.Time).IsRequired();
            b.Property(x => x.ReqBuildingId).HasDefaultValue(null);

            b
                .HasOne(x => x.Building)
                .WithMany()
                .HasForeignKey(x => x.ReqBuildingId);

            base.Configure(b);
        }
    }
}
