using System;
using Hinode.Izumi.Data.Enums.EffectEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Commands
{
    public record AddEffectToUserCommand(
            long UserId,
            EffectCategory Category,
            Effect Effect,
            DateTimeOffset? Expiration = null)
        : IRequest;
}
