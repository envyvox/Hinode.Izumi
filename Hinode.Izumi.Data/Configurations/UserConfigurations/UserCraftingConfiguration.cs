using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserCraftingConfiguration : EntityTypeConfigurationBase<UserCrafting>
    {
        public override void Configure(EntityTypeBuilder<UserCrafting> b)
        {
            b.HasIndex(x => new {x.UserId, x.CraftingId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Crafting)
                .WithMany()
                .HasForeignKey(x => x.CraftingId);

            base.Configure(b);
        }
    }
}
