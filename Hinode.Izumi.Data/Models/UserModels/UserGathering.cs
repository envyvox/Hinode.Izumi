namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserGathering : EntityBase
    {
        public long UserId { get; set; }
        public long GatheringId { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
        public virtual Gathering Gathering { get; set; }
    }
}
