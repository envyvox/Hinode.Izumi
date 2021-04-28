using System;

namespace Hinode.Izumi.Framework.EF
{
    public interface IEntityBase
    {
        long Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
