using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.FoodWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.FoodWebService.Impl
{
    [InjectableService]
    public class FoodWebService : IFoodWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public FoodWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<FoodWebModel>> GetAllFood() =>
            await _con.GetConnection()
                .QueryAsync<FoodWebModel>(@"
                    select * from foods
                    order by id");

        public async Task<FoodWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FoodWebModel>(@"
                    select * from foods
                    where id = @id",
                    new {id});

        public async Task<FoodWebModel> Upsert(FoodWebModel model)
        {
            // сбрасываем кэш
            _cache.Remove(string.Format(CacheExtensions.FoodKey, model.Id));

            var query = model.Id == 0
                ? @"
                    insert into foods(name, mastery, time, recipe_sellable, event)
                    values (@name, @mastery, @time, @recipeSellable, @event)
                    returning *"
                : @"
                    update foods
                    set name = @name,
                        mastery = @mastery,
                        time = @time,
                        recipe_sellable = @recipeSellable,
                        event = @event,
                        updated_at = now()
                    where id = @id
                    returning *";

            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FoodWebModel>(query,
                    new
                    {
                        id = model.Id,
                        name = model.Name,
                        mastery = model.Mastery,
                        time = model.Time,
                        recipeSellable = model.RecipeSellable,
                        @event = model.Event
                    });
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from foods
                    where id = @id",
                    new {id});
    }
}
