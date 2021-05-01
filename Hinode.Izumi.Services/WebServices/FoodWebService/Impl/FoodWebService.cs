using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
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
        private readonly IIngredientService _ingredientService;
        private readonly ICalculationService _calc;

        public FoodWebService(IConnectionManager con, IMemoryCache cache, IIngredientService ingredientService,
            ICalculationService calc)
        {
            _con = con;
            _cache = cache;
            _ingredientService = ingredientService;
            _calc = calc;
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
                    insert into foods(name, mastery, time)
                    values (@name, @mastery, @time)
                    returning *"
                : @"
                    update foods
                    set name = @name,
                        mastery = @mastery,
                        time = @time,
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
                        time = model.Time
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
