using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserDrinkConfiguration : EntityTypeConfigurationBase<UserDrink>
    {
        public override void Configure(EntityTypeBuilder<UserDrink> b)
        {
            b.HasIndex(x => new {x.UserId, x.DrinkId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Drink)
                .WithMany()
                .HasForeignKey(x => x.DrinkId);

            base.Configure(b);
        }
    }
}
