using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserBox : EntityBase
    {
        public long UserId { get; set; }
        public Box Box { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
    }
}
