using System;
using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CooldownService.Commands
{
    public record AddCooldownToUserCommand(long UserId, Cooldown Cooldown, DateTimeOffset Expiration) : IRequest;
}
