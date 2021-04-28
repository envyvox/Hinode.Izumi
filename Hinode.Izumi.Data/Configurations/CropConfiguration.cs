using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class CropConfiguration : EntityTypeConfigurationBase<Crop>
    {
        public override void Configure(EntityTypeBuilder<Crop> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Price).IsRequired();

            b
                .HasOne(x => x.Seed)
                .WithMany()
                .HasForeignKey(x => x.SeedId);

            base.Configure(b);
        }
    }
}
