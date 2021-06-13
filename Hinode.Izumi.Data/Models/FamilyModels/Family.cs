using Hinode.Izumi.Data.Enums.FamilyEnums;

namespace Hinode.Izumi.Data.Models.FamilyModels
{
    public class Family : EntityBase
    {
        public FamilyStatus Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
