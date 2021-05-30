using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserHangfireJobConfiguration : EntityTypeConfigurationBase<UserHangfireJob>
    {
        public override void Configure(EntityTypeBuilder<UserHangfireJob> b)
        {
            b.HasIndex(x => new {x.UserId, x.Action, x.JobId}).IsUnique();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            base.Configure(b);
        }
    }
}
