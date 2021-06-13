using System;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserCooldown : EntityBase
    {
        public long UserId { get; set; }
        public Cooldown Cooldown { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public virtual User User { get; set; }
    }
}
