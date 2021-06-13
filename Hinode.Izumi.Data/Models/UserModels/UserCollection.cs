using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserCollection : EntityBase
    {
        public long UserId { get; set; }
        public CollectionCategory Category { get; set; }
        public long ItemId { get; set; }
        public Event Event { get; set; }
        public virtual User User { get; set; }
    }
}
