using System;

namespace Hinode.Izumi.Services.Database
{
    public class EntityBaseModel
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
