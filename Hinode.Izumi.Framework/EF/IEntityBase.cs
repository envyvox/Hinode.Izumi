using System;

namespace Hinode.Izumi.Framework.EF
{
    public interface IEntityBase
    {
        long Id { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}
