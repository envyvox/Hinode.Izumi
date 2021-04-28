using Hinode.Izumi.Data.Models;
using Hinode.Izumi.Framework.EF;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Data.Configurations
{
    public class ProjectIngredientConfiguration : EntityTypeConfigurationBase<ProjectIngredient>
    {
        public override void Configure(EntityTypeBuilder<ProjectIngredient> b)
        {
            b.HasIndex(x => new {x.ProjectId, x.Category, x.IngredientId}).IsUnique();

            b.Property(x => x.Amount).IsRequired();

            b
                .HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId);

            base.Configure(b);
        }
    }
}
