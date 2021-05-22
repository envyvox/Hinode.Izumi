using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.GatheringService.Handlers
{
    public class GetGatheringHandler : IRequestHandler<GetGatheringQuery, GatheringRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetGatheringHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<GatheringRecord> Handle(GetGatheringQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.GatheringKey, request.Id),
                out GatheringRecord gathering)) return gathering;

            gathering = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<GatheringRecord>(@"
                    select * from gatherings
                    where id = @id",
                    new {id = request.Id});

            _cache.Set(string.Format(CacheExtensions.GatheringKey, request.Id), gathering,
                CacheExtensions.DefaultCacheOptions);

            return gathering;
        }
    }
}
