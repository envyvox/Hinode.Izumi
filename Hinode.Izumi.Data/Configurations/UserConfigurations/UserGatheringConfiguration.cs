using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserGatheringConfiguration : EntityTypeConfigurationBase<UserGathering>
    {
        public override void Configure(EntityTypeBuilder<UserGathering> b)
        {
            b.HasIndex(x => new {x.UserId, x.GatheringId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Gathering)
                .WithMany()
                .HasForeignKey(x => x.GatheringId);

            base.Configure(b);
        }
    }
}
