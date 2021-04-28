using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class EmoteConfiguration : EntityTypeConfigurationBase<Emote>
    {
        public override void Configure(EntityTypeBuilder<Emote> b)
        {
            b.HasIndex(x => new {x.Id, x.Name}).IsUnique();

            b.Property(x => x.Code).IsRequired();

            base.Configure(b);
        }
    }
}
