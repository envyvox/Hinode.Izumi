using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.FoodService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.FoodService.Impl
{
    [InjectableService]
    public class FoodService : IFoodService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public FoodService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<FoodModel[]> GetAllFood() =>
            (await _con.GetConnection()
                .QueryAsync<FoodModel>(@"
                    select * from foods
                    where event = false
                    order by id"))
            .ToArray();

        public async Task<FoodModel[]> GetAllRecipeSellableFood() =>
            (await _con.GetConnection()
                .QueryAsync<FoodModel>(@"
                    select * from foods
                    where recipe_sellable = true
                    order by id"))
            .ToArray();

        public async Task<FoodModel> GetFood(long id)
        {
            // проверяем блюдо в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.FoodKey, id), out FoodModel food)) return food;

            // получаем блюдо из базы
            food = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FoodModel>(@"
                    select * from foods
                    where id = @id",
                    new {id});

            // добавляем блюдо в кэш
            _cache.Set(string.Format(CacheExtensions.FoodKey, id), food, CacheExtensions.DefaultCacheOptions);

            // возвращаем блюдо
            return food;
        }

        public async Task<FoodModel[]> GetUserRecipes(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<FoodModel>(@"
                    select f.* from user_recipes as uc
                        inner join foods f
                            on f.id = uc.food_id
                    where uc.user_id = @userId
                    order by f.id",
                    new {userId}))
            .ToArray();

        public async Task<bool> CheckUserRecipe(long userId, long foodId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_recipes
                    where user_id = @userId
                      and food_id = @foodId",
                    new {userId, foodId});

        public async Task AddRecipeToUser(long userId, long foodId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_recipes(user_id, food_id)
                    values (@userId, @foodId)
                    on conflict (user_id, food_id) do nothing",
                    new {userId, foodId});
    }
}
