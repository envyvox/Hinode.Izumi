using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.AlcoholService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.AlcoholService.Impl
{
    [InjectableService]
    public class AlcoholService : IAlcoholService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;


        public AlcoholService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<AlcoholModel[]> GetAllAlcohol() =>
            (await _con.GetConnection()
                .QueryAsync<AlcoholModel>(@"
                    select * from alcohols"))
            .ToArray();

        public async Task<AlcoholModel> GetAlcohol(long id)
        {
            // проверяем алкоголь в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.AlcoholKey, id), out AlcoholModel alcohol))
                return alcohol;

            // получаем алкоголь из базы
            alcohol = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholModel>(@"
                    select * from alcohols
                    where id = @id",
                    new {id});

            // добавляем алкоголь в кэш
            _cache.Set(string.Format(CacheExtensions.AlcoholKey, id), alcohol, CacheExtensions.DefaultCacheOptions);

            // возвращаем алкоголь
            return alcohol;
        }
    }
}
