using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.GatheringService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.GatheringService.Queries
{
    public record GetAllGatheringsQuery(Event CurrentEvent = Event.None) : IRequest<GatheringRecord[]>;

    public class GetAllGatheringsHandler : IRequestHandler<GetAllGatheringsQuery, GatheringRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllGatheringsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<GatheringRecord[]> Handle(GetAllGatheringsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<GatheringRecord>(@"
                        select * from gatherings
                        where event = @currentEvent
                           or event = @eventNone",
                        new {currentEvent = request.CurrentEvent, eventNone = Event.None}))
                .ToArray();
        }
    }
}
