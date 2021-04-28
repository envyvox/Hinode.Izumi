using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserProductConfiguration : EntityTypeConfigurationBase<UserProduct>
    {
        public override void Configure(EntityTypeBuilder<UserProduct> b)
        {
            b.HasIndex(x => new {x.UserId, x.ProductId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId);

            base.Configure(b);
        }
    }
}
