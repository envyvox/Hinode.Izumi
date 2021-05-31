using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserPremiumPropertiesConfiguration : EntityTypeConfigurationBase<UserPremiumProperties>
    {
        public override void Configure(EntityTypeBuilder<UserPremiumProperties> b)
        {
            b.HasIndex(x => x.UserId).IsUnique();

            b.Property(x => x.CommandColor).HasDefaultValue("36393F");

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
