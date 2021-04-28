using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.FishWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.FishWebService.Impl
{
    [InjectableService]
    public class FishWebService : IFishWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public FishWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<FishWebModel>> GetAllFish() =>
            await _con.GetConnection()
                .QueryAsync<FishWebModel>(@"
                    select * from fishes
                    order by id");

        public async Task<FishWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FishWebModel>(@"
                    select * from fishes
                    where id = @id",
                    new {id});

        public async Task<FishWebModel> Upsert(FishWebModel model)
        {
            // сбрасываем кэш
            _cache.Remove(string.Format(CacheExtensions.FishKey, model.Id));
            // переписываем сезоны в массив номеров (из-за проблем каста необходимо сделать это вручную)
            var seasons = model.Seasons
                .Select(season => (int) season)
                .ToArray();

            var query = model.Id == 0
                ? @"
                    insert into fishes(name, rarity, seasons, weather, times_day, price)
                    values (@name, @rarity, @seasons, @weather, @timesDay, @price)
                    returning *"
                : @"
                    update fishes
                    set name = @name,
                        rarity = @rarity,
                        seasons = @seasons,
                        weather = @weather,
                        times_day = @timesDay,
                        price = @price,
                        updated_at = now()
                    where id = @id
                    returning *";

            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FishWebModel>(query,
                    new
                    {
                        id = model.Id,
                        name = model.Name,
                        rarity = model.Rarity,
                        seasons,
                        weather = model.Weather,
                        timesDay = model.TimesDay,
                        price = model.Price
                    });
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from fishes
                    where id = @id",
                    new {id});
    }
}
