using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class MovementConfiguration : EntityTypeConfigurationBase<Movement>
    {
        public override void Configure(EntityTypeBuilder<Movement> b)
        {
            b.HasIndex(x => x.UserId).IsUnique();

            b.Property(x => x.Departure).IsRequired();
            b.Property(x => x.Destination).IsRequired();
            b.Property(x => x.Arrival).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
