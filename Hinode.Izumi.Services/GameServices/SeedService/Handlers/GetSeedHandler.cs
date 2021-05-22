using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using Hinode.Izumi.Services.GameServices.SeedService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.SeedService.Handlers
{
    public class GetSeedHandler : IRequestHandler<GetSeedQuery, SeedRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetSeedHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<SeedRecord> Handle(GetSeedQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.SeedKey, request.Id), out SeedRecord seed))
                return seed;

            seed = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<SeedRecord>(@"
                    select * from seeds
                    where id = @id",
                    new {id = request.Id});

            _cache.Set(string.Format(CacheExtensions.SeedKey, request.Id), seed, CacheExtensions.DefaultCacheOptions);

            return seed;
        }
    }
}
