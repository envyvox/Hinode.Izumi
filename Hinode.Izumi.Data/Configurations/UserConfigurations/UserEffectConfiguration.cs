using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserEffectConfiguration : EntityTypeConfigurationBase<UserEffect>
    {
        public override void Configure(EntityTypeBuilder<UserEffect> b)
        {
            b.HasIndex(x => new {x.UserId, x.Effect}).IsUnique();

            b.Property(x => x.Category).IsRequired();
            b.Property(x => x.Expiration);

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
