using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.DrinkService.Queries;
using Hinode.Izumi.Services.GameServices.DrinkService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.DrinkService.Handlers
{
    public class GetDrinkHandler : IRequestHandler<GetDrinkQuery, DrinkRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetDrinkHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<DrinkRecord> Handle(GetDrinkQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.DrinkKey, request.Id), out DrinkRecord drink))
                return drink;

            drink = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<DrinkRecord>(@"
                    select * from drinks
                    where id = @id",
                    new {id = request.Id});

            _cache.Set(string.Format(CacheExtensions.DrinkKey, request.Id), drink, CacheExtensions.DefaultCacheOptions);

            return drink;
        }
    }
}
