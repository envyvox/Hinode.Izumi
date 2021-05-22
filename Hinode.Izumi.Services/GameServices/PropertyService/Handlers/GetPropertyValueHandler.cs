using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.PropertyService.Handlers
{
    public class GetPropertyValueHandler : IRequestHandler<GetPropertyValueQuery, long>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetPropertyValueHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<long> Handle(GetPropertyValueQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.PropertyKey, request.Property), out long value))
                return value;

            value = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select value from world_properties
                    where property = @property",
                    new {property = request.Property});

            _cache.Set(string.Format(CacheExtensions.PropertyKey, request.Property), value,
                CacheExtensions.DefaultCacheOptions);

            return value;
        }
    }
}
