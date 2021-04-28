using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserTrainingConfiguration : EntityTypeConfigurationBase<UserTraining>
    {
        public override void Configure(EntityTypeBuilder<UserTraining> b)
        {
            b.HasIndex(x => x.UserId).IsUnique();

            b.Property(x => x.Step).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
