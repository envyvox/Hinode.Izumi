using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.CropService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.CropService.Impl
{
    [InjectableService]
    public class CropService : ICropService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CropService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<CropModel[]> GetAllCrops() =>
            (await _con.GetConnection()
                .QueryAsync<CropModel>(@"
                    select c.*, s.season from crops as c
                        inner join seeds s
                            on s.id = c.seed_id"))
            .ToArray();

        public async Task<CropModel> GetCrop(long id)
        {
            // проверяем урожай в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.CropKey, id), out CropModel crop)) return crop;

            // получаем урожай из базы
            crop = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CropModel>(@"
                    select c.*, s.season from crops as c
                        inner join seeds s
                            on s.id = c.seed_id
                    where c.id = @id",
                    new {id});

            // добавляем урожай в кэш
            _cache.Set(string.Format(CacheExtensions.CropKey, id), crop, CacheExtensions.DefaultCacheOptions);

            // возвращаем урожай
            return crop;
        }

        public async Task<CropModel> GetCropBySeedId(long seedId)
        {
            // проверяем урожай в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.CropBySeedKey, seedId), out CropModel crop))
                return crop;

            // получаем урожай из базы
            crop = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CropModel>(@"
                    select c.*, s.season from crops as c
                        inner join seeds s
                            on s.id = c.seed_id
                    where c.seed_id = @seedId",
                    new {seedId});

            // добавляем урожай в кэш
            _cache.Set(string.Format(CacheExtensions.CropBySeedKey, seedId), crop, CacheExtensions.DefaultCacheOptions);

            // возвращаем урожай
            return crop;
        }

        public async Task<CropModel> GetRandomCrop() =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CropModel>(@"
                    select * from crops
                    order by random()
                    limit 1");
    }
}
