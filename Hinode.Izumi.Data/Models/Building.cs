using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    public class Building : EntityBase
    {
        public Enums.Building Type { get; set; }
        public long? ProjectId { get; set; }
        public BuildingCategory Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Project Project { get; set; }
    }
}
