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
    public class GetAlcoholPropertiesHandler : IRequestHandler<GetAlcoholPropertiesQuery, AlcoholPropertyRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetAlcoholPropertiesHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<AlcoholPropertyRecord> Handle(GetAlcoholPropertiesQuery request,
            CancellationToken cancellationToken)
        {
            var (alcoholId, property) = request;

            if (_cache.TryGetValue(string.Format(CacheExtensions.AlcoholPropertyKey, alcoholId, property),
                out AlcoholPropertyRecord properties)) return properties;

            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholPropertyRecord>(@"
                    select * from alcohol_properties
                    where alcohol_id = @alcoholId
                      and property = @property",
                    new {alcoholId, property});

            _cache.Set(string.Format(CacheExtensions.AlcoholPropertyKey, alcoholId, property), properties,
                CacheExtensions.DefaultCacheOptions);

            return properties;
        }
    }
}
