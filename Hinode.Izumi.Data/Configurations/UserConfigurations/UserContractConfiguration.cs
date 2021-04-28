using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserContractConfiguration : EntityTypeConfigurationBase<UserContract>
    {
        public override void Configure(EntityTypeBuilder<UserContract> b)
        {
            b.HasIndex(x => x.UserId).IsUnique();

            b.Property(x => x.ContractId).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Contract)
                .WithMany()
                .HasForeignKey(x => x.ContractId);

            base.Configure(b);
        }
    }
}
