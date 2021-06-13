using System;
using Hinode.Izumi.Data.Enums.EffectEnums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserEffect : EntityBase
    {
        public long UserId { get; set; }
        public EffectCategory Category { get; set; }
        public Effect Effect { get; set; }
        public DateTimeOffset? Expiration { get; set; }
        public virtual User User { get; set; }
    }
}
