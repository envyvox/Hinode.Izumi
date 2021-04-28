using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.SeedService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.SeedService.Impl
{
    [InjectableService]
    public class SeedService : ISeedService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;
        private readonly ILocalizationService _local;

        public SeedService(IConnectionManager con, IMemoryCache cache, ILocalizationService local)
        {
            _con = con;
            _cache = cache;
            _local = local;
        }

        public async Task<SeedModel[]> GetSeed(Season season) =>
            (await _con.GetConnection()
                .QueryAsync<SeedModel>(@"
                    select * from seeds
                    where season = @season
                    order by id",
                    new {season}))
            .ToArray();

        public async Task<SeedModel> GetSeed(long id)
        {
            // проверяем семя в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.SeedKey, id), out SeedModel seed)) return seed;

            // получаем семя из базы
            seed = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<SeedModel>(@"
                    select * from seeds
                    where id = @id",
                    new {id});

            // добавляем семя в кэш
            _cache.Set(string.Format(CacheExtensions.SeedKey, id), seed, CacheExtensions.DefaultCacheOptions);

            // возвращаем семя
            return seed;
        }

        public async Task<SeedModel> GetSeed(string namePattern)
        {
            // проверяем семя в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.SeedByNameKey, namePattern), out SeedModel seed))
                return seed;

            // получаем локализацию семени
            var localization = await _local.GetLocalizationByLocalizedWord(LocalizationCategory.Seed, namePattern);

            // получаем семя из базы
            seed = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<SeedModel>(@"
                    select * from seeds
                    where name = @name",
                    new {name = localization.Name});

            // добавляем семя в кэш
            _cache.Set(string.Format(CacheExtensions.SeedByNameKey, namePattern), seed,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем семя
            return seed;
        }
    }
}
