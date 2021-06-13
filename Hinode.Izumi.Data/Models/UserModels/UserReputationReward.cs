using Hinode.Izumi.Data.Enums.ReputationEnums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserReputationReward : EntityBase
    {
        public long UserId { get; set; }
        public Reputation Reputation { get; set; }
        public long Amount { get; set; }
        public virtual User User { get; set; }
    }
}
