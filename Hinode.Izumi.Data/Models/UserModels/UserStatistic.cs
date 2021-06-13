using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserStatistic : EntityBase
    {
        public long UserId { get; set; }
        public Statistic Statistic { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
    }
}
