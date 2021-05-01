using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.FishService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.FishService.Impl
{
    [InjectableService]
    public class FishService : IFishService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public FishService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<FishModel[]> GetAllFish() =>
            (await _con.GetConnection()
                .QueryAsync<FishModel>(@"
                    select * from fishes
                    order by id"))
            .ToArray();

        public async Task<FishModel[]> GetAllFish(Season season) =>
            (await _con.GetConnection()
                .QueryAsync<FishModel>(@"
                    select * from fishes
                    where @season = any(seasons)
                       or @anySeason = any(seasons)
                    order by id",
                    new {season, anySeason = Season.Any}))
            .ToArray();

        public async Task<FishModel> GetFish(long id)
        {
            // проверяем рыбу в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.FishKey, id), out FishModel fish)) return fish;

            // получаем рыбу из базы
            fish = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FishModel>(@"
                    select * from fishes
                    where id = @id",
                    new {id});

            // добавляем рыбу в кэш
            _cache.Set(string.Format(CacheExtensions.FishKey, id), fish, CacheExtensions.DefaultCacheOptions);

            // возвращаем рыбу
            return fish;
        }

        public async Task<FishModel>
            GetRandomFish(TimesDay timesDay, Season season, Weather weather, FishRarity rarity) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FishModel>(@"
                    select * from fishes
                    where (times_day in (0, @timesDay))
                        and (weather in (0, @weather))
                        and (0 = any(seasons) or @season = any(seasons))
                        and rarity = @rarity
                    order by random()
                    limit 1",
                    new {timesDay, season, weather, rarity});

        public async Task<FishModel> GetRandomFish(FishRarity rarity) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FishModel>(@"
                    select * from fishes
                    where rarity = @rarity
                    order by random()
                    limit 1",
                    new {rarity});
    }
}
