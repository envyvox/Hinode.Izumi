using Hinode.Izumi.Data.Enums.ReputationEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Commands
{
    public record CheckReputationRewardsCommand(long UserId, Reputation Reputation) : IRequest;
}
