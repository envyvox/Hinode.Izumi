using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class ImageConfiguration : EntityTypeConfigurationBase<Image>
    {
        public override void Configure(EntityTypeBuilder<Image> b)
        {
            b.Property(x => x.Type).IsRequired();
            b.Property(x => x.Url).IsRequired();

            base.Configure(b);
        }
    }
}
