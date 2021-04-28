using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.Options;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.DrinkService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.MarketService;
using Microsoft.Extensions.Options;

namespace Hinode.Izumi.Services.BackgroundJobs.MarketJob
{
    [InjectableService]
    public class MarketJob : IMarketJob
    {
        private readonly IOptions<DiscordOptions> _options;
        private readonly IConnectionManager _con;
        private readonly ICraftingService _craftingService;
        private readonly ICalculationService _calc;
        private readonly IIngredientService _ingredientService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly IFoodService _foodService;
        private readonly IMarketService _marketService;

        public MarketJob(IOptions<DiscordOptions> options, IConnectionManager con, ICraftingService craftingService,
            ICalculationService calc, IIngredientService ingredientService, IAlcoholService alcoholService,
            IDrinkService drinkService, IFoodService foodService, IMarketService marketService)
        {
            _options = options;
            _con = con;
            _craftingService = craftingService;
            _calc = calc;
            _ingredientService = ingredientService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _foodService = foodService;
            _marketService = marketService;
        }

        public async Task DailyMarketReset()
        {
            // получаем Id Изуми
            var izumiId = (long) _options.Value.BotId;
            // каждая категория товаров добавляется по своему
            foreach (var category in Enum.GetValues(typeof(MarketCategory))
                .Cast<MarketCategory>())
            {
                switch (category)
                {
                    case MarketCategory.Gathering:

                        // добавляем или обновляем все заявки собирательских ресурсов
                        await _con.GetConnection()
                            .ExecuteAsync(@"
                                insert into market_requests(user_id, category, item_id, price, amount, selling)
                                select
                                       @izumiId, @category, i.id, i.price, 9999, false
                                from gatherings i
                                on conflict (user_id, category, item_id) do update
                                    set price = excluded.price,
                                        amount = 9999,
                                        updated_at = now()",
                                new {izumiId, category});

                        break;
                    case MarketCategory.Crafting:

                        // получаем все изготавливаемые предметы
                        var craftings = await _craftingService.GetAllCraftings();
                        // теперь нужно добавить или обновить каждый из них
                        foreach (var crafting in craftings)
                        {
                            // получаем цену NPC изготавливаемого предмета
                            var price = await _calc.NpcPrice(category,
                                // получаем себестоимость изготавливаемого предмета
                                await _ingredientService.GetCraftingCostPrice(crafting.Id));
                            // добавляем или обновляем изготавливаемый предмет на рынок
                            await _marketService.AddOrUpdateMarketRequest(
                                izumiId, category, crafting.Id, price, 9999, false);
                        }

                        break;
                    case MarketCategory.Alcohol:

                        // получаем весь алкоголь
                        var alcohols = await _alcoholService.GetAllAlcohol();
                        // теперь нужно добавить или обновить каждый из них
                        foreach (var alcohol in alcohols)
                        {
                            // получаем цену NPC алкоголя
                            var price = await _calc.NpcPrice(category,
                                // получаем себестоимость алкоголя
                                await _ingredientService.GetAlcoholCostPrice(alcohol.Id));
                            // добавляем или обновляем алкоголь на рынок
                            await _marketService.AddOrUpdateMarketRequest(
                                izumiId, category, alcohol.Id, price, 9999, false);
                        }

                        break;
                    case MarketCategory.Drink:

                        // получаем все напитки
                        var drinks = await _drinkService.GetAllDrinks();
                        // теперь нужно добавить или обновить каждый из них
                        foreach (var drink in drinks)
                        {
                            // получаем цену NPC напитка
                            var price = await _calc.NpcPrice(category,
                                // получаем себестоимость напитка
                                await _ingredientService.GetDrinkCostPrice(drink.Id));
                            // добавляем или обновляем напиток на рынок
                            await _marketService.AddOrUpdateMarketRequest(
                                izumiId, category, drink.Id, price, 9999, false);
                        }

                        break;
                    case MarketCategory.Food:

                        // получаем все блюда
                        var foods = await _foodService.GetAllFood();
                        // теперь нужно добавить или обновить каждое из них
                        foreach (var food in foods)
                        {
                            // получаем цену NPC блюда
                            var price = await _calc.NpcPrice(category,
                                // получаем себестоимость блюда
                                await _ingredientService.GetFoodCostPrice(food.Id));
                            // добавляем или обновляем блюдо на рынок
                            await _marketService.AddOrUpdateMarketRequest(
                                izumiId, category, food.Id, price, 9999, false);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
