using System;
using Hinode.Izumi.Framework.EF;

namespace Hinode.Izumi.Data.Models
{
    public class EntityBase: IEntityBase
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
