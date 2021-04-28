using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.FoodIngredientWebService.Models;

namespace Hinode.Izumi.Services.WebServices.FoodIngredientWebService.Impl
{
    [InjectableService]
    public class FoodIngredientWebService : IFoodIngredientWebService
    {
        private readonly IConnectionManager _con;

        public FoodIngredientWebService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<IEnumerable<FoodIngredientWebModel>> GetAllFoodIngredients() =>
            await _con.GetConnection()
                .QueryAsync<FoodIngredientWebModel>(@"
                    select fi.*, f.name as FoodName,
                       case
                           when fi.category = 2
                               then (select name from products where id = fi.ingredient_id)
                           when fi.category = 3
                               then (select name from craftings where id = fi.ingredient_id)
                           when fi.category = 6
                               then (select name from crops where id = fi.ingredient_id)
                           when fi.category = 7 then
                               (select name from foods where id = fi.ingredient_id)
                           end as IngredientName
                    from food_ingredients as fi
                        inner join foods f
                            on f.id = fi.food_id
                    order by fi.food_id");

        public async Task<IEnumerable<FoodIngredientWebModel>> GetFoodIngredients(long foodId) =>
            await _con.GetConnection()
                .QueryAsync<FoodIngredientWebModel>(@"
                    select fi.*, f.name as FoodName,
                       case
                           when fi.category = 2
                               then (select name from products where id = fi.ingredient_id)
                           when fi.category = 3
                               then (select name from craftings where id = fi.ingredient_id)
                           when fi.category = 6
                               then (select name from crops where id = fi.ingredient_id)
                           when fi.category = 7 then
                               (select name from foods where id = fi.ingredient_id)
                           end as IngredientName
                    from food_ingredients as fi
                        inner join foods f
                            on f.id = fi.food_id
                    where fi.food_id = @foodId",
                    new {foodId});

        public async Task<FoodIngredientWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FoodIngredientWebModel>(@"
                    select * from food_ingredients
                    where id = @id",
                    new {id});

        public async Task<FoodIngredientWebModel> Upsert(FoodIngredientWebModel model)
        {
            var query = model.Id == 0
                ? @"
                    insert into food_ingredients(food_id, category, ingredient_id, amount)
                    values (@foodId, @category, @ingredientId, @amount)
                    returning *"
                : @"
                    update food_ingredients
                    set amount = @amount,
                        updated_at = now()
                    where id = @id
                    returning *";

            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FoodIngredientWebModel>(query,
                    new
                    {
                        id = model.Id,
                        foodId = model.FoodId,
                        category = model.Category,
                        ingredientId = model.IngredientId,
                        amount = model.Amount
                    });
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from food_ingredients
                    where id = @id",
                    new {id});
    }
}
