using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class ContractConfiguration : EntityTypeConfigurationBase<Contract>
    {
        public override void Configure(EntityTypeBuilder<Contract> b)
        {
            b.HasIndex(x => x.Name).IsUnique();

            b.Property(x => x.Location).IsRequired();
            b.Property(x => x.Description).IsRequired();
            b.Property(x => x.Time).IsRequired();
            b.Property(x => x.Currency).IsRequired();
            b.Property(x => x.Reputation).IsRequired();
            b.Property(x => x.Energy).IsRequired();

            base.Configure(b);
        }
    }
}
