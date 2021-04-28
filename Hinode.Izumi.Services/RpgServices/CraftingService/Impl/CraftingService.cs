using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.CraftingService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.CraftingService.Impl
{
    [InjectableService]
    public class CraftingService : ICraftingService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CraftingService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<CraftingModel[]> GetAllCraftings() =>
            (await _con.GetConnection()
                .QueryAsync<CraftingModel>(@"
                    select * from craftings"))
            .ToArray();

        public async Task<CraftingModel> GetCrafting(long id)
        {
            // проверяем изготавливаемый предмет в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.CraftingKey, id), out CraftingModel crafting))
                return crafting;

            // получаем изготавливаемый предмет из базы
            crafting = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingModel>(@"
                    select * from craftings
                    where id = @id",
                    new {id});

            // добавляем изготавливаемый предмет в кэш
            _cache.Set(string.Format(CacheExtensions.CraftingKey, id), crafting, CacheExtensions.DefaultCacheOptions);

            // возвращаем изготавливаемый предмет
            return crafting;
        }
    }
}
