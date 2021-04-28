using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.CraftingIngredientWebService.Models;

namespace Hinode.Izumi.Services.WebServices.CraftingIngredientWebService.Impl
{
    [InjectableService]
    public class CraftingIngredientWebService : ICraftingIngredientWebService
    {
        private readonly IConnectionManager _con;

        public CraftingIngredientWebService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<IEnumerable<CraftingIngredientWebModel>> GetAllCraftingIngredients() =>
            await _con.GetConnection()
                .QueryAsync<CraftingIngredientWebModel>(@"
                    select ci.*, c.name as CraftingName,
                        case
                            when ci.category = 1
                                then (select name from gatherings where id = ci.ingredient_id)
                            when ci.category = 2
                                then (select name from products where id = ci.ingredient_id)
                            when ci.category = 3
                                then (select name from craftings where id = ci.ingredient_id)
                            when ci.category = 4
                                then (select name from alcohols where id = ci.ingredient_id)
                            when ci.category = 5
                                then (select name from drinks where id = ci.ingredient_id)
                            when ci.category = 6
                                then (select name from crops where id = ci.ingredient_id)
                            when ci.category = 7
                                then (select name from foods where id = ci.ingredient_id)
                            end as IngredientName
                    from crafting_ingredients as ci
                        inner join craftings c
                            on c.id = ci.crafting_id
                    order by ci.crafting_id");

        public async Task<IEnumerable<CraftingIngredientWebModel>> GetCraftingIngredients(long craftingId) =>
            await _con.GetConnection()
                .QueryAsync<CraftingIngredientWebModel>(@"
                    select ci.*, c.name as CraftingName,
                        case
                            when ci.category = 1
                                then (select name from gatherings where id = ci.ingredient_id)
                            when ci.category = 2
                                then (select name from products where id = ci.ingredient_id)
                            when ci.category = 3
                                then (select name from craftings where id = ci.ingredient_id)
                            when ci.category = 4
                                then (select name from alcohols where id = ci.ingredient_id)
                            when ci.category = 5
                                then (select name from drinks where id = ci.ingredient_id)
                            when ci.category = 6
                                then (select name from crops where id = ci.ingredient_id)
                            when ci.category = 7
                                then (select name from foods where id = ci.ingredient_id)
                            end as IngredientName
                    from crafting_ingredients as ci
                        inner join craftings c
                            on c.id = ci.crafting_id
                    where ci.crafting_id = @craftingId",
                    new {craftingId});

        public async Task<CraftingIngredientWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingIngredientWebModel>(@"
                    select * from crafting_ingredients
                    where id = @id",
                    new {id});

        public async Task<CraftingIngredientWebModel> Upsert(CraftingIngredientWebModel model)
        {
            var query = model.Id == 0
                ? @"
                    insert into crafting_ingredients(crafting_id, category, ingredient_id, amount)
                    values (@craftingId, @category, @ingredientId, @amount)
                    returning *"
                : @"
                    update crafting_ingredients
                    set amount = @amount,
                        updated_at = now()
                    where id = @id
                    returning *";

            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingIngredientWebModel>(query,
                    new
                    {
                        id = model.Id,
                        craftingId = model.CraftingId,
                        category = model.Category,
                        ingredientId = model.IngredientId,
                        amount = model.Amount
                    });
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from crafting_ingredients
                    where id = @id",
                    new {id});
    }
}
