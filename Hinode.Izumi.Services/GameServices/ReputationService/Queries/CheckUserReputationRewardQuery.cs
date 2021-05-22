using Hinode.Izumi.Data.Enums.ReputationEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Queries
{
    public record CheckUserReputationRewardQuery(long UserId, Reputation Reputation, long Amount) : IRequest<bool>;
}
