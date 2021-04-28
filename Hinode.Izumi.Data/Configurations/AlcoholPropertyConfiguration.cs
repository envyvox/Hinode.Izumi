using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class AlcoholPropertyConfiguration : EntityTypeConfigurationBase<AlcoholProperty>
    {
        public override void Configure(EntityTypeBuilder<AlcoholProperty> b)
        {
            b.HasIndex(x => new {x.AlcoholId, x.Property}).IsUnique();

            b.Property(x => x.Mastery0).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery50).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery100).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery150).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery200).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery250).IsRequired().HasDefaultValue(0);

            b
                .HasOne(x => x.Alcohol)
                .WithMany()
                .HasForeignKey(x => x.AlcoholId);

            base.Configure(b);
        }
    }
}
