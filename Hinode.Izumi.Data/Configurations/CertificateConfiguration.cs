using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class CertificateConfiguration : EntityTypeConfigurationBase<Certificate>
    {
        public override void Configure(EntityTypeBuilder<Certificate> b)
        {
            b.HasIndex(x => x.Type).IsUnique();

            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Description).IsRequired();
            b.Property(x => x.Price).IsRequired();

            base.Configure(b);
        }
    }
}
