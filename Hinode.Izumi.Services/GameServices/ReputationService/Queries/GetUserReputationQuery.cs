using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Services.GameServices.ReputationService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Queries
{
    public record GetUserReputationQuery(long UserId, Reputation Reputation) : IRequest<UserReputationRecord>;
}
