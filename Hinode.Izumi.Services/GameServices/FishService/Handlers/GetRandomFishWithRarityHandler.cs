using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FishService.Handlers
{
    public class GetRandomFishWithRarityHandler : IRequestHandler<GetRandomFishWithRarityQuery, FishRecord>
    {
        private readonly IConnectionManager _con;

        public GetRandomFishWithRarityHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FishRecord> Handle(GetRandomFishWithRarityQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FishRecord>(@"
                    select * from fishes
                    where rarity = @rarity
                    order by random()
                    limit 1",
                    new {rarity = request.Rarity});
        }
    }
}
