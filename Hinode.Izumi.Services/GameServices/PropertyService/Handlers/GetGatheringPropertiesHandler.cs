using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.PropertyService.Handlers
{
    public class GetGatheringPropertiesHandler : IRequestHandler<GetGatheringPropertiesQuery, GatheringPropertyRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetGatheringPropertiesHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<GatheringPropertyRecord> Handle(GetGatheringPropertiesQuery request,
            CancellationToken cancellationToken)
        {
            var (gatheringId, property) = request;

            if (_cache.TryGetValue(string.Format(CacheExtensions.GatheringPropertyKey, gatheringId, property),
                out GatheringPropertyRecord properties)) return properties;

            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<GatheringPropertyRecord>(@"
                    select * from gathering_properties
                    where gathering_id = @gatheringId
                      and property = @property",
                    new {gatheringId, property});

            _cache.Set(string.Format(CacheExtensions.GatheringPropertyKey, gatheringId, property), properties,
                CacheExtensions.DefaultCacheOptions);

            return properties;
        }
    }
}
