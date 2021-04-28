using Hinode.Izumi.Data.Models.UserModels;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations.UserConfigurations
{
    public class UserProjectConfiguration : EntityTypeConfigurationBase<UserProject>
    {
        public override void Configure(EntityTypeBuilder<UserProject> b)
        {
            b.HasIndex(x => new {x.UserId, x.ProjectId}).IsUnique();

            b
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            b
                .HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId);

            base.Configure(b);
        }
    }
}
