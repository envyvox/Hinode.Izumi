using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.GameServices.LocationService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.LocationService.Handlers
{
    public class GetTransitHandler : IRequestHandler<GetTransitQuery, TransitRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetTransitHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<TransitRecord> Handle(GetTransitQuery request, CancellationToken cancellationToken)
        {
            var (departure, destination) = request;

            if (_cache.TryGetValue(string.Format(CacheExtensions.TransitKey, departure, destination),
                out TransitRecord transit)) return transit;


            transit = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<TransitRecord>(@"
                    select * from transits
                    where departure = @departure
                      and destination = @destination",
                    new {departure, destination});


            if (transit is not null)
            {
                _cache.Set(string.Format(CacheExtensions.TransitKey, departure, destination), transit,
                    CacheExtensions.DefaultCacheOptions);
                return transit;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.Transit.Parse()));
            return null;
        }
    }
}
