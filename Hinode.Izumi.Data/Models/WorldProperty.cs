using Hinode.Izumi.Data.Enums.PropertyEnums;

namespace Hinode.Izumi.Data.Models
{
    public class WorldProperty : EntityBase
    {
        public PropertyCategory PropertyCategory { get; set; }
        public Property Property { get; set; }
        public long Value { get; set; }
    }
}
