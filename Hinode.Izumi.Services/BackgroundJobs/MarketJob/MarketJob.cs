using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.Options;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.DrinkService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.MarketService.Commands;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hinode.Izumi.Services.BackgroundJobs.MarketJob
{
    [InjectableService]
    public class MarketJob : IMarketJob
    {
        private readonly IOptions<DiscordOptions> _options;
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public MarketJob(IOptions<DiscordOptions> options, IConnectionManager con, IMediator mediator)
        {
            _options = options;
            _con = con;
            _mediator = mediator;
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
                        var craftings = await _mediator.Send(new GetAllCraftingsQuery());
                        // теперь нужно добавить или обновить каждый из них
                        foreach (var crafting in craftings)
                        {
                            // получаем цену NPC изготавливаемого предмета
                            var price = await _mediator.Send(new GetNpcPriceQuery(category,
                                // получаем себестоимость изготавливаемого предмета
                                await _mediator.Send(new GetCraftingCostPriceQuery(crafting.Id))));
                            // добавляем или обновляем изготавливаемый предмет на рынок
                            await _mediator.Send(new CreateOrUpdateMarketRequestCommand(
                                izumiId, category, crafting.Id, price, 9999, false));
                        }

                        break;
                    case MarketCategory.Alcohol:

                        // получаем весь алкоголь
                        var alcohols = await _mediator.Send(new GetAllAlcoholQuery());
                        // теперь нужно добавить или обновить каждый из них
                        foreach (var alcohol in alcohols)
                        {
                            // получаем цену NPC алкоголя
                            var price = await _mediator.Send(new GetNpcPriceQuery(category,
                                // получаем себестоимость алкоголя
                                await _mediator.Send(new GetAlcoholCostPriceQuery(alcohol.Id))));
                            // добавляем или обновляем алкоголь на рынок
                            await _mediator.Send(new CreateOrUpdateMarketRequestCommand(
                                izumiId, category, alcohol.Id, price, 9999, false));
                        }

                        break;
                    case MarketCategory.Drink:

                        // получаем все напитки
                        var drinks = await _mediator.Send(new GetAllDrinksQuery());
                        // теперь нужно добавить или обновить каждый из них
                        foreach (var drink in drinks)
                        {
                            // получаем цену NPC напитка
                            var price = await _mediator.Send(new GetNpcPriceQuery(category,
                                // получаем себестоимость напитка
                                await _mediator.Send(new GetDrinkCostPriceQuery(drink.Id))));
                            // добавляем или обновляем напиток на рынок
                            await _mediator.Send(new CreateOrUpdateMarketRequestCommand(
                                izumiId, category, drink.Id, price, 9999, false));
                        }

                        break;
                    case MarketCategory.Food:

                        // получаем все блюда
                        var foods = await _mediator.Send(new GetAllFoodQuery());
                        // теперь нужно добавить или обновить каждое из них
                        foreach (var food in foods)
                        {
                            // получаем цену NPC блюда
                            var price = await _mediator.Send(new GetNpcPriceQuery(category,
                                // получаем себестоимость блюда
                                await _mediator.Send(new GetFoodCostPriceQuery(food.Id))));
                            // добавляем или обновляем блюдо на рынок
                            await _mediator.Send(new CreateOrUpdateMarketRequestCommand(
                                izumiId, category, food.Id, price, 9999, false));
                        }

                        break;
                    case MarketCategory.Crop:

                        var crops = await _mediator.Send(new GetAllCropsQuery());
                        foreach (var crop in crops)
                        {
                            await _mediator.Send(new CreateOrUpdateMarketRequestCommand(
                                izumiId, category, crop.Id, crop.Price, 9999, false));
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
