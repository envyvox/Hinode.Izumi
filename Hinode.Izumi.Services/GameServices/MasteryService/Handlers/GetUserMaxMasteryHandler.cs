using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Handlers
{
    public class GetUserMaxMasteryHandler : IRequestHandler<GetUserMaxMasteryQuery, double>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public GetUserMaxMasteryHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<double> Handle(GetUserMaxMasteryQuery request, CancellationToken cancellationToken)
        {
            var reputations = Enum.GetValues(typeof(Reputation))
                .Cast<Reputation>()
                .ToArray();
            var userReputations = await _mediator.Send(
                new GetUserReputationsQuery(request.UserId), cancellationToken);

            var userAverageReputation =
                reputations.Sum(reputation =>
                    userReputations.ContainsKey(reputation) ? userReputations[reputation].Amount : 0) /
                reputations.Length;

            return ReputationStatusHelper
                .GetReputationStatus(userAverageReputation)
                .MaxMastery();
        }
    }
}
