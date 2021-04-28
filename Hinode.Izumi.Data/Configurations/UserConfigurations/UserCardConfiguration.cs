using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserCardConfiguration : EntityTypeConfigurationBase<UserCard>
    {
        public override void Configure(EntityTypeBuilder<UserCard> b)
        {
            b.HasIndex(x => new {x.UserId, x.CardId}).IsUnique();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Card)
                .WithMany()
                .HasForeignKey(x => x.CardId);

            base.Configure(b);
        }
    }
}
