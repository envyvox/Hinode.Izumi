using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.FamilyModels
{
    public class FamilyCurrency : EntityBase
    {
        public long FamilyId { get; set; }
        public Currency Currency { get; set; }
        public long Amount { get; set; }
        public virtual Family Family { get; set; }
    }
}
