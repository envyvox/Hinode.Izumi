using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FishService.Handlers
{
    public class GetRandomFishWithParamsHandler : IRequestHandler<GetRandomFishWithParamsQuery, FishRecord>
    {
        private readonly IConnectionManager _con;

        public GetRandomFishWithParamsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FishRecord> Handle(GetRandomFishWithParamsQuery request, CancellationToken cancellationToken)
        {
            var (timesDay, season, weather, rarity) = request;

            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FishRecord>(@"
                    select * from fishes
                    where (times_day in (0, @timesDay))
                        and (weather in (0, @weather))
                        and (0 = any(seasons) or @season = any(seasons))
                        and rarity = @rarity
                    order by random()
                    limit 1",
                    new {timesDay, season, weather, rarity});
        }
    }
}
