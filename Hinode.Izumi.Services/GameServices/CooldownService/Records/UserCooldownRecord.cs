using System;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.CooldownService.Records
{
    public record UserCooldownRecord(Cooldown Cooldown, DateTimeOffset Expiration)
    {
        public UserCooldownRecord() : this(default, default)
        {
        }
    }
}
