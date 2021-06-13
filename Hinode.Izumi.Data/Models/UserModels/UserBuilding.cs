namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserBuilding : EntityBase
    {
        public long UserId { get; set; }
        public long BuildingId { get; set; }
        public virtual User User { get; set; }
        public virtual Building Building { get; set; }
    }
}
