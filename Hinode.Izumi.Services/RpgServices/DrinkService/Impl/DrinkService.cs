using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.DrinkService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.DrinkService.Impl
{
    [InjectableService]
    public class DrinkService : IDrinkService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public DrinkService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<DrinkModel[]> GetAllDrinks() =>
            (await _con.GetConnection()
                .QueryAsync<DrinkModel>(@"
                    select * from drinks"))
            .ToArray();

        public async Task<DrinkModel> GetDrink(long id)
        {
            // проверяем напиток в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.DrinkKey, id), out DrinkModel drink)) return drink;

            // получаем напиток из базы
            drink = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<DrinkModel>(@"
                    select * from drinks
                    where id = @id",
                    new {id});

            // добавляем напиток в кэш
            _cache.Set(string.Format(CacheExtensions.DrinkKey, id), drink, CacheExtensions.DefaultCacheOptions);

            // возвращаем напиток
            return drink;
        }
    }
}
