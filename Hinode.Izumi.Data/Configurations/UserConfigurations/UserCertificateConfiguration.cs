using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserCertificateConfiguration : EntityTypeConfigurationBase<UserCertificate>
    {
        public override void Configure(EntityTypeBuilder<UserCertificate> b)
        {
            b.HasIndex(x => new {x.UserId, x.CertificateId}).IsUnique();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Certificate)
                .WithMany()
                .HasForeignKey(x => x.CertificateId);

            base.Configure(b);
        }
    }
}
