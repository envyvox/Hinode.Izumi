using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserCropConfiguration : EntityTypeConfigurationBase<UserCrop>
    {
        public override void Configure(EntityTypeBuilder<UserCrop> b)
        {
            b.HasIndex(x => new {x.UserId, x.CropId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Crop)
                .WithMany()
                .HasForeignKey(x => x.CropId);

            base.Configure(b);
        }
    }
}
