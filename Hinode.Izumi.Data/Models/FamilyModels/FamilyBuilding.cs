namespace Hinode.Izumi.Data.Models.FamilyModels
{
    public class FamilyBuilding : EntityBase
    {
        public long FamilyId { get; set; }
        public long BuildingId { get; set; }
        public virtual Family Family { get; set; }
        public virtual Building Building { get; set; }
    }
}
