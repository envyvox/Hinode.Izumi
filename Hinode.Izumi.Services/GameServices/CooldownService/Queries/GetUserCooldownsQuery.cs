using System.Collections.Generic;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.CooldownService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CooldownService.Queries
{
    public record GetUserCooldownsQuery(long UserId) : IRequest<Dictionary<Cooldown, UserCooldownRecord>>;
}
