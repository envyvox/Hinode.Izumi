using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.MarketService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketCreateBuyRequestCommand
{
    [InjectableService]
    public class MarketCreateBuyRequestCommand : IMarketCreateBuyRequestCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IMarketService _marketService;
        private readonly IImageService _imageService;
        private readonly IInventoryService _inventoryService;
        private readonly ILocalizationService _local;
        private readonly ICalculationService _calc;
        private readonly IIngredientService _ingredientService;
        private readonly IGatheringService _gatheringService;
        private readonly IPropertyService _propertyService;

        public MarketCreateBuyRequestCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IMarketService marketService, IImageService imageService, IInventoryService inventoryService,
            ILocalizationService local, ICalculationService calc, IIngredientService ingredientService,
            IGatheringService gatheringService, IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _marketService = marketService;
            _imageService = imageService;
            _inventoryService = inventoryService;
            _local = local;
            _calc = calc;
            _ingredientService = ingredientService;
            _gatheringService = gatheringService;
            _propertyService = propertyService;
        }

        public async Task Execute(SocketCommandContext context, MarketCategory category, string pattern, long price,
            long amount = 1)
        {
            // для начала нужно найти товар по локализированному названию
            var localization = await _local.GetLocalizationByLocalizedWord(category, pattern);
            var itemId = localization.ItemId;
            var itemName = localization.Name;

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем валюту пользователя
            var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);
            // ищем заявку пользователя на этот предмет
            var userRequest = await _marketService.GetMarketUserRequest((long) context.User.Id, category, itemId);

            // если такая заявка есть - пользователь не может создать еще одну
            if (userRequest != null)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketRequestAlready.Parse(
                    userRequest.Id, emotes.GetEmoteOrBlank(itemName), _local.Localize(itemName))));
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

                        var gathering = await _gatheringService.GetGathering(itemId);
                        npcPrice = gathering.Price;

                        break;
                    case MarketCategory.Crafting:

                        npcPrice = await _calc.NpcPrice(category,
                            await _ingredientService.GetCraftingCostPrice(itemId));

                        break;
                    case MarketCategory.Alcohol:

                        npcPrice = await _calc.NpcPrice(category,
                            await _ingredientService.GetAlcoholCostPrice(itemId));

                        break;
                    case MarketCategory.Drink:

                        npcPrice = await _calc.NpcPrice(category,
                            await _ingredientService.GetDrinkCostPrice(itemId));

                        break;
                    case MarketCategory.Food:

                        npcPrice = await _calc.NpcPrice(category,
                            await _ingredientService.GetFoodCostPrice(itemId));

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
                    var userRequests = await _marketService.GetMarketUserRequest((long) context.User.Id, category);
                    // получаем максимальное количество заявок на рынке в одной категории
                    var maxRequestsLength = await _propertyService.GetPropertyValue(Property.MarketMaxRequests);

                    // проверяем чтобы пользователь не выставил больше заявок, чем это разрешено
                    if (userRequests.Length >= maxRequestsLength)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.MarketRequestGroupLimit.Parse()));
                    }
                    else
                    {
                        // отнимаем у пользователя валюту на оплату заявки
                        await _inventoryService.RemoveItemFromUser(
                            (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                            price * amount);
                        // добавляем заявку на рынок
                        await _marketService.AddOrUpdateMarketRequest(
                            (long) context.User.Id, category, itemId, price, amount, false);

                        var embed = new EmbedBuilder()
                            // баннер рынка
                            .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket))
                            // подтверждаем успешное создание заявки
                            .WithDescription(IzumiReplyMessage.MarketBuyRequestSuccess.Parse(
                                emotes.GetEmoteOrBlank(itemName), amount, _local.Localize(itemName, amount),
                                emotes.GetEmoteOrBlank(Currency.Ien.ToString()), price,
                                _local.Localize(Currency.Ien.ToString(), price)));

                        await _discordEmbedService.SendEmbed(context.User, embed);
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
