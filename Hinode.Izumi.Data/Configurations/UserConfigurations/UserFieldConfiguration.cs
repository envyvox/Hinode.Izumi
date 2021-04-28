using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserFieldConfiguration : EntityTypeConfigurationBase<UserField>
    {
        public override void Configure(EntityTypeBuilder<UserField> b)
        {
            b.HasIndex(x => new {x.UserId, x.FieldId}).IsUnique();

            b.Property(x => x.State).IsRequired().HasDefaultValue(FieldState.Empty);
            b.Property(x => x.SeedId).HasDefaultValue(null);
            b.Property(x => x.Progress).IsRequired().HasDefaultValue(0);
            b.Property(x => x.ReGrowth).IsRequired().HasDefaultValue(false);

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Seed)
                .WithMany()
                .HasForeignKey(x => x.SeedId);

            base.Configure(b);
        }
    }
}
