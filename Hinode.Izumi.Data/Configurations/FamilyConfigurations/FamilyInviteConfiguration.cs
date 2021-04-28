using Hinode.Izumi.Data.Models.FamilyModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.FamilyConfigurations
{
    public class FamilyInviteConfiguration : EntityTypeConfigurationBase<FamilyInvite>
    {
        public override void Configure(EntityTypeBuilder<FamilyInvite> b)
        {
            b.HasIndex(x => new {x.FamilyId, x.UserId}).IsUnique();

            b
                .HasOne(x => x.Family)
                .WithMany()
                .HasForeignKey(x => x.FamilyId);

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
