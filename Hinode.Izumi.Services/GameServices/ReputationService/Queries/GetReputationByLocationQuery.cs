using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Queries
{
    public record GetReputationByLocationQuery(Location Location) : IRequest<Reputation>;

    public class GetReputationByLocationHandler : IRequestHandler<GetReputationByLocationQuery, Reputation>
    {
        public async Task<Reputation> Handle(GetReputationByLocationQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(request.Location switch
            {
                Location.Capital => Reputation.Capital,
                Location.Garden => Reputation.Garden,
                Location.Seaport => Reputation.Seaport,
                Location.Castle => Reputation.Castle,
                Location.Village => Reputation.Village,
                _ => throw new ArgumentOutOfRangeException()
            });
        }
    }
}
