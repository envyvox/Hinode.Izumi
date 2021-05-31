using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.DrinkService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.MarketService.Commands;
using Hinode.Izumi.Services.GameServices.MarketService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketCreateBuyRequestCommand
{
    [InjectableService]
    public class MarketCreateBuyRequestCommand : IMarketCreateBuyRequestCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public MarketCreateBuyRequestCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, MarketCategory category, string pattern, long price,
            long amount = 1)
        {
            // для начала нужно найти товар по локализированному названию
            var localization = await _local.GetLocalizationByLocalizedWord(category, pattern);
            var itemId = localization.ItemId;
            var itemName = localization.Name;

            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем валюту пользователя
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));
            // ищем заявку пользователя на этот предмет
            var userRequest = await _mediator.Send(new GetMarketUserRequestQuery(
                (long) context.User.Id, category, itemId));

            // если такая заявка есть - пользователь не может создать еще одну
            if (userRequest is not null)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketRequestAlready.Parse(
                    userRequest.Id, emotes.GetEmoteOrBlank(itemName), _local.Localize(category, itemId))));
            }
            // проверяем хватает ли пользователю денег на оплату заявки
            else if (userCurrency.Amount < price * amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketBuyRequestNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString()))));
            }
            else
            {
                // получаем стоимость товара у NPC (она же минимальная цена)
                long npcPrice;
                switch (category)
                {
                    case MarketCategory.Gathering:

                        var gathering = await _mediator.Send(new GetGatheringQuery(itemId));
                        npcPrice = gathering.Price;

                        break;
                    case MarketCategory.Crafting:

                        npcPrice = await _mediator.Send(new GetNpcPriceQuery(category,
                            await _mediator.Send(new GetCraftingCostPriceQuery(itemId))));

                        break;
                    case MarketCategory.Alcohol:

                        npcPrice = await _mediator.Send(new GetNpcPriceQuery(category,
                            await _mediator.Send(new GetAlcoholCostPriceQuery(itemId))));

                        break;
                    case MarketCategory.Drink:

                        npcPrice = await _mediator.Send(new GetNpcPriceQuery(category,
                            await _mediator.Send(new GetDrinkCostPriceQuery(itemId))));

                        break;
                    case MarketCategory.Food:

                        npcPrice = await _mediator.Send(new GetNpcPriceQuery(category,
                            await _mediator.Send(new GetFoodCostPriceQuery(itemId))));

                        break;
                    case MarketCategory.Crop:

                        var crop = await _mediator.Send(new GetCropByIdQuery(itemId));
                        npcPrice = crop.Price;

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(category), category, null);
                }

                // проверяем что цена не ниже минимальной стоимости товара
                if (price < npcPrice)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.MarketRequestMinCost.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), npcPrice,
                        _local.Localize(Currency.Ien.ToString(), npcPrice))));
                }
                else
                {
                    // получаем заявки пользователя в этой категории
                    var userRequests = await _mediator.Send(new GetMarketUserRequestsInCategoryQuery(
                        (long) context.User.Id, category));
                    // получаем максимальное количество заявок на рынке в одной категории
                    var maxRequestsLength = await _mediator.Send(new GetPropertyValueQuery(
                        Property.MarketMaxRequests));

                    // проверяем чтобы пользователь не выставил больше заявок, чем это разрешено
                    if (userRequests.Length >= maxRequestsLength)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.MarketRequestGroupLimit.Parse()));
                    }
                    else
                    {
                        // отнимаем у пользователя валюту на оплату заявки
                        await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                            (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                            price * amount));
                        // добавляем заявку на рынок
                        await _mediator.Send(new CreateOrUpdateMarketRequestCommand(
                            (long) context.User.Id, category, itemId, price, amount, false));

                        var embed = new EmbedBuilder()
                            // баннер рынка
                            .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalMarket)))
                            // подтверждаем успешное создание заявки
                            .WithDescription(IzumiReplyMessage.MarketBuyRequestSuccess.Parse(
                                emotes.GetEmoteOrBlank(itemName), amount, _local.Localize(category, itemId, amount),
                                emotes.GetEmoteOrBlank(Currency.Ien.ToString()), price,
                                _local.Localize(Currency.Ien.ToString(), price)));

                        await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
