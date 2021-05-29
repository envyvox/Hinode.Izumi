using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FishService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.FishService.Queries
{
    public record GetFishQuery(long Id) : IRequest<FishRecord>;

    public class GetFishHandler : IRequestHandler<GetFishQuery, FishRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetFishHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<FishRecord> Handle(GetFishQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.FishKey, request.Id), out FishRecord fish))
                return fish;

            fish = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FishRecord>(@"
                    select * from fishes
                    where id = @id",
                    new {id = request.Id});

            _cache.Set(string.Format(CacheExtensions.FishKey, request.Id), fish, CacheExtensions.DefaultCacheOptions);

            return fish;
        }
    }
}
