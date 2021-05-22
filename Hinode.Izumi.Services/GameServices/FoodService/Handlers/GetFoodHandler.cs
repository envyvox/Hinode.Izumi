using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.FoodService.Handlers
{
    public class GetFoodHandler : IRequestHandler<GetFoodQuery, FoodRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetFoodHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<FoodRecord> Handle(GetFoodQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.FoodKey, request.Id), out FoodRecord food))
                return food;

            food = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FoodRecord>(@"
                    select * from foods
                    where id = @id",
                    new {id = request.Id});

            _cache.Set(string.Format(CacheExtensions.FoodKey, request.Id), food, CacheExtensions.DefaultCacheOptions);

            return food;
        }
    }
}
