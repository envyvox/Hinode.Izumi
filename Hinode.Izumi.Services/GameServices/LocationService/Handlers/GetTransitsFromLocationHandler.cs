using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.GameServices.LocationService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.LocationService.Handlers
{
    public class GetTransitsFromLocationHandler : IRequestHandler<GetTransitsFromLocationQuery, TransitRecord[]>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetTransitsFromLocationHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<TransitRecord[]> Handle(GetTransitsFromLocationQuery request,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.TransitsLocationKey, request.Departure),
                out TransitRecord[] transits)) return transits;

            transits = (await _con.GetConnection()
                    .QueryAsync<TransitRecord>(@"
                        select * from transits
                        where departure = @departure",
                        new {departure = request.Departure}))
                .ToArray();

            _cache.Set(string.Format(CacheExtensions.TransitsLocationKey, request.Departure), transits,
                CacheExtensions.DefaultCacheOptions);

            return transits;
        }
    }
}
