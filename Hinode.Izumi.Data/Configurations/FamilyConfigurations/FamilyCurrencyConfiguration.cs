using Hinode.Izumi.Data.Models.FamilyModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.FamilyConfigurations
{
    public class FamilyCurrencyConfiguration : EntityTypeConfigurationBase<FamilyCurrency>
    {
        public override void Configure(EntityTypeBuilder<FamilyCurrency> b)
        {
            b.HasIndex(x => new {x.FamilyId, x.Currency}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.Family)
                .WithMany()
                .HasForeignKey(x => x.FamilyId);

            base.Configure(b);
        }
    }
}
