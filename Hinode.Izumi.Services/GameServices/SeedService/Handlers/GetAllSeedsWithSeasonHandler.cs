using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using Hinode.Izumi.Services.GameServices.SeedService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.SeedService.Handlers
{
    public class GetAllSeedsWithSeasonHandler : IRequestHandler<GetAllSeedsWithSeasonQuery, SeedRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllSeedsWithSeasonHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<SeedRecord[]> Handle(GetAllSeedsWithSeasonQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<SeedRecord>(@"
                        select * from seeds
                        where season = @season
                        order by id",
                        new {season = request.Season}))
                .ToArray();
        }
    }
}
