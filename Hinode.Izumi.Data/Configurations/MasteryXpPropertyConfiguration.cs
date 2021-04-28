using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class MasteryXpPropertyConfiguration : EntityTypeConfigurationBase<MasteryXpProperty>
    {
        public override void Configure(EntityTypeBuilder<MasteryXpProperty> b)
        {
            b.HasIndex(x => x.Property).IsUnique();

            b.Property(x => x.Mastery0).IsRequired();
            b.Property(x => x.Mastery50).IsRequired();
            b.Property(x => x.Mastery100).IsRequired();
            b.Property(x => x.Mastery150).IsRequired();
            b.Property(x => x.Mastery200).IsRequired();
            b.Property(x => x.Mastery250).IsRequired();

            base.Configure(b);
        }
    }
}
