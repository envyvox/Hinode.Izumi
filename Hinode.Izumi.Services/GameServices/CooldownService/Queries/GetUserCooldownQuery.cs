using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.CooldownService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CooldownService.Queries
{
    public record GetUserCooldownQuery(long UserId, Cooldown Cooldown) : IRequest<UserCooldownRecord>;
}
