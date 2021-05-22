using System;
using Hinode.Izumi.Data.Enums.EffectEnums;

namespace Hinode.Izumi.Services.GameServices.EffectService.Records
{
    public record UserEffectRecord(
        long UserId,
        EffectCategory Category,
        Effect Effect,
        DateTimeOffset? Expiration)
    {
        public UserEffectRecord() : this(default, default, default, default)
        {
        }
    }
}
