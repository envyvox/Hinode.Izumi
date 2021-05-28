using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.GatheringService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.GatheringService.Queries
{
    public record GetGatheringByNameQuery(string Name) : IRequest<GatheringRecord>;

    public class GetGatheringByNameHandler : IRequestHandler<GetGatheringByNameQuery, GatheringRecord>
    {
        private readonly IConnectionManager _con;

        public GetGatheringByNameHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<GatheringRecord> Handle(GetGatheringByNameQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<GatheringRecord>(@"
                    select * from gatherings
                    where name = @name",
                    new {name = request.Name});
        }
    }
}
