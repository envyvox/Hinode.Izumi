using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserAlcoholConfiguration : EntityTypeConfigurationBase<UserAlcohol>
    {
        public override void Configure(EntityTypeBuilder<UserAlcohol> b)
        {
            b.HasIndex(x => new {x.UserId, x.AlcoholId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Alcohol)
                .WithMany()
                .HasForeignKey(x => x.AlcoholId);

            base.Configure(b);
        }
    }
}
