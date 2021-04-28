using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.GatheringService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.GatheringService.Impl
{
    [InjectableService]
    public class GatheringService : IGatheringService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GatheringService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<GatheringModel[]> GetAllGatherings() =>
            (await _con.GetConnection()
                .QueryAsync<GatheringModel>(@"
                    select * from gatherings"))
            .ToArray();

        public async Task<GatheringModel[]> GetGathering(Location location) =>
            (await _con.GetConnection()
                .QueryAsync<GatheringModel>(@"
                    select * from gatherings
                    where location = @location",
                    new {location}))
            .ToArray();

        public async Task<GatheringModel> GetGathering(long id)
        {
            // проверяем собирательский ресурс в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.GatheringKey, id), out GatheringModel gathering))
                return gathering;

            // получаем собирательский ресурс из базы
            gathering = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<GatheringModel>(@"
                    select * from gatherings
                    where id = @id",
                    new {id});

            // добавляем собирательский ресурс в кэш
            _cache.Set(string.Format(CacheExtensions.GatheringKey, id), gathering, CacheExtensions.DefaultCacheOptions);

            // возвращаем собирательский ресурс
            return gathering;
        }
    }
}
