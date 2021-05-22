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
    public class GetMasteryXpPropertiesHandler : IRequestHandler<GetMasteryXpPropertiesQuery, MasteryXpPropertyRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetMasteryXpPropertiesHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<MasteryXpPropertyRecord> Handle(GetMasteryXpPropertiesQuery request,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.MasteryXpPropertyKey, request.Property),
                out MasteryXpPropertyRecord properties)) return properties;

            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MasteryXpPropertyRecord>(@"
                    select * from mastery_xp_properties
                    where property = @property",
                    new {property = request.Property});

            _cache.Set(string.Format(CacheExtensions.MasteryXpPropertyKey, request.Property), properties,
                CacheExtensions.DefaultCacheOptions);

            return properties;
        }
    }
}
