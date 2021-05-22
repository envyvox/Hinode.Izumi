using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FishService.Handlers
{
    public class GetAllFishBySeasonHandler : IRequestHandler<GetAllFishBySeasonQuery, FishRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllFishBySeasonHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FishRecord[]> Handle(GetAllFishBySeasonQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FishRecord>(@"
                        select * from fishes
                        where @season = any(seasons)
                           or @anySeason = any(seasons)
                        order by id",
                        new {season = request.Season, anySeason = Season.Any}))
                .ToArray();
        }
    }
}
