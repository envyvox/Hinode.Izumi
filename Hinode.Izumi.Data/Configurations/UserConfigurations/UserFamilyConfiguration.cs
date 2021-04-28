using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserFamilyConfiguration : EntityTypeConfigurationBase<UserFamily>
    {
        public override void Configure(EntityTypeBuilder<UserFamily> b)
        {
            b.HasIndex(x => x.UserId).IsUnique();

            b.Property(x => x.FamilyId).IsRequired();
            b.Property(x => x.Status).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Family)
                .WithMany()
                .HasForeignKey(x => x.FamilyId);

            base.Configure(b);
        }
    }
}
