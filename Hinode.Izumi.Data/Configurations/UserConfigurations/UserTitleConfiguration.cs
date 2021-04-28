using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserTitleConfiguration : EntityTypeConfigurationBase<UserTitle>
    {
        public override void Configure(EntityTypeBuilder<UserTitle> b)
        {
            b.HasIndex(x => new {x.UserId, x.Title}).IsUnique();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
