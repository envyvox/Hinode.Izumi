using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserCollectionConfiguration : EntityTypeConfigurationBase<UserCollection>
    {
        public override void Configure(EntityTypeBuilder<UserCollection> b)
        {
            b.HasIndex(x => new {x.UserId, x.Category, x.ItemId}).IsUnique();

            b.Property(x => x.Event).IsRequired().HasDefaultValue(Event.None);

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
