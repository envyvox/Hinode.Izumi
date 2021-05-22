using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Queries
{
    public record GetReputationByLocationQuery(Location Location) : IRequest<Reputation>;
}
