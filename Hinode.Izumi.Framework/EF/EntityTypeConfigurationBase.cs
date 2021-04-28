using Hinode.Izumi.Framework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hinode.Izumi.Framework.EF
{
    public abstract class EntityTypeConfigurationBase<T> : IEntityTypeConfiguration<T> where T : class, IEntityBase
    {
        public virtual void Configure(EntityTypeBuilder<T> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.CreatedAt).HasDefaultValueSql("now()");
            b.Property(x => x.UpdatedAt).HasDefaultValueSql("now()");

            var name = typeof(T).Name;
            var ending = "s";

            if (
                name.EndsWith("s") ||
                name.EndsWith("sh") ||
                name.EndsWith("ch") ||
                name.EndsWith("x") ||
                name.EndsWith("o")
            )
            {
                ending = "es";
            }
            else if (name.EndsWith("y"))
            {
                name = name.Remove(name.Length - 1, 1);
                ending = "ies";
            }
            else if (name.EndsWith("f"))
            {
                name = name.Remove(name.Length - 1, 1);
                ending = "ves";
            }
            else if (name.EndsWith("fe"))
            {
                name = name.Remove(name.Length - 2, 2);
                ending = "ves";
            }

            var fixedName = name + ending;

            //to remove postgre quotes
            b.ToTable(fixedName.ToSnakeCase());

            var entity = b.Metadata;

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnBaseName().ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName().ToSnakeCase());
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
            }
        }
    }
}
