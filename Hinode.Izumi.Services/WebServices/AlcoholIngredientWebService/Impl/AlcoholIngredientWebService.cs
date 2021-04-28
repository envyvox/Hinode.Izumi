using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.AlcoholIngredientWebService.Models;

namespace Hinode.Izumi.Services.WebServices.AlcoholIngredientWebService.Impl
{
    [InjectableService]
    public class AlcoholIngredientWebService : IAlcoholIngredientWebService
    {
        private readonly IConnectionManager _con;

        public AlcoholIngredientWebService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<IEnumerable<AlcoholIngredientWebModel>> GetAllAlcoholIngredients() =>
            await _con.GetConnection()
                .QueryAsync<AlcoholIngredientWebModel>(@"
                    select ai.*, a.name as AlcoholName,
                        case
                            when ai.category = 1
                                then (select name from gatherings where id = ai.ingredient_id)
                            when ai.category = 2
                                then (select name from products where id = ai.ingredient_id)
                            when ai.category = 3
                                then (select name from craftings where id = ai.ingredient_id)
                            when ai.category = 4
                                then (select name from alcohols where id = ai.ingredient_id)
                            when ai.category = 5
                                then (select name from drinks where id = ai.ingredient_id)
                            when ai.category = 6
                                then (select name from crops where id = ai.ingredient_id)
                            when ai.category = 7
                                then (select name from foods where id = ai.ingredient_id)
                            end as IngredientName
                    from alcohol_ingredients as ai
                        inner join alcohols a
                            on a.id = ai.alcohol_id
                    order by ai.alcohol_id");

        public async Task<IEnumerable<AlcoholIngredientWebModel>> GetAlcoholIngredients(long alcoholId) =>
            await _con.GetConnection()
                .QueryAsync<AlcoholIngredientWebModel>(@"
                    select ai.*, a.name as AlcoholName,
                        case
                            when ai.category = 1
                                then (select name from gatherings where id = ai.ingredient_id)
                            when ai.category = 2
                                then (select name from products where id = ai.ingredient_id)
                            when ai.category = 3
                                then (select name from craftings where id = ai.ingredient_id)
                            when ai.category = 4
                                then (select name from alcohols where id = ai.ingredient_id)
                            when ai.category = 5
                                then (select name from drinks where id = ai.ingredient_id)
                            when ai.category = 6
                                then (select name from crops where id = ai.ingredient_id)
                            when ai.category = 7
                                then (select name from foods where id = ai.ingredient_id)
                            end as IngredientName
                    from alcohol_ingredients as ai
                        inner join alcohols a
                            on a.id = ai.alcohol_id
                    where ai.alcohol_id = @alcoholId",
                    new {alcoholId});

        public async Task<AlcoholIngredientWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholIngredientWebModel>(@"
                    select * from alcohol_ingredients
                    where id = @id",
                    new {id});

        public async Task<AlcoholIngredientWebModel> Update(AlcoholIngredientWebModel model) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholIngredientWebModel>(@"
                    insert into alcohol_ingredients(alcohol_id, category, ingredient_id, amount)
                    values (@alcoholId, @category, @ingredientId, @amount)
                    on conflict (alcohol_id, category, ingredient_id) do update
                        set alcohol_id = @alcoholId,
                            category = @category,
                            ingredient_id = @ingredientId,
                            amount = @amount,
                            updated_at = now()
                    returning *",
                    new
                    {
                        alcoholId = model.AlcoholId,
                        category = model.Category,
                        ingredientId = model.IngredientId,
                        amount = model.Amount
                    });

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from alcohol_ingredients
                    where id = @id",
                    new {id});
    }
}
