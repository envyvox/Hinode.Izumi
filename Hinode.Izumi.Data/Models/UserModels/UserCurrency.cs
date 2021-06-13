using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserCurrency : EntityBase
    {
        public long UserId { get; set; }
        public Currency Currency { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
    }
}
