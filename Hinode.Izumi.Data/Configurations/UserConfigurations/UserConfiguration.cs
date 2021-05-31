using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserConfiguration : EntityTypeConfigurationBase<User>
    {
        public override void Configure(EntityTypeBuilder<User> b)
        {
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.About);
            b.Property(x => x.Title).IsRequired().HasDefaultValue(Title.Newbie);
            b.Property(x => x.Gender).IsRequired().HasDefaultValue(Gender.None);
            b.Property(x => x.Location).IsRequired().HasDefaultValue(Location.Capital);
            b.Property(x => x.Energy).IsRequired().HasDefaultValue(100);
            b.Property(x => x.Points).IsRequired().HasDefaultValue(0);
            b.Property(x => x.Premium).IsRequired().HasDefaultValue(false);

            base.Configure(b);
        }
    }
}
