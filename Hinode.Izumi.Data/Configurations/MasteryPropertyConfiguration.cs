using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class MasteryPropertyConfiguration : EntityTypeConfigurationBase<MasteryProperty>
    {
        public override void Configure(EntityTypeBuilder<MasteryProperty> b)
        {
            b.HasIndex(x => x.Property).IsUnique();

            b.Property(x => x.PropertyCategory).IsRequired();
            b.Property(x => x.Mastery0).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery50).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery100).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery150).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery200).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Mastery250).IsRequired().HasDefaultValue(0);

            base.Configure(b);
        }
    }
}
