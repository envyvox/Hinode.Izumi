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

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketCreateSellRequestCommand
{
    [InjectableService]
    public class MarketCreateSellRequestCommand : IMarketCreateSellRequestCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IMarketService _marketService;
        private readonly IImageService _imageService;
        private readonly IInventoryService _inventoryService;
        private readonly ILocalizationService _local;
        private readonly ICalculationService _calc;
        private readonly IGatheringService _gatheringService;
        private readonly IIngredientService _ingredientService;
        private readonly IPropertyService _propertyService;

        public MarketCreateSellRequestCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IMarketService marketService, IImageService imageService, IInventoryService inventoryService,
            ILocalizationService local, ICalculationService calc, IGatheringService gatheringService,
            IIngredientService ingredientService, IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _marketService = marketService;
            _imageService = imageService;
            _inventoryService = inventoryService;
            _local = local;
            _calc = calc;
            _gatheringService = gatheringService;
            _ingredientService = ingredientService;
            _propertyService = propertyService;
        }

        public async Task Execute(SocketCommandContext context, MarketCategory category, string pattern, long price,
            long amount = 1)
        {
            // для начала нужно найти товар по локализированному названию
            var localization = await _local.GetLocalizationByLocalizedWord(category, pattern);
            var itemId = localization.ItemId;
            var itemName = localization.Name;

            // получаем количество товара у пользователя
            long userAmount;
            switch (category)
            {
                case MarketCategory.Gathering:

                    var userGathering = await _inventoryService.GetUserGathering((long) context.User.Id, itemId);
                    userAmount = userGathering.Amount;

                    break;
                case MarketCategory.Crafting:

                    var userCrafting = await _inventoryService.GetUserCrafting((long) context.User.Id, itemId);
                    userAmount = userCrafting.Amount;

                    break;
                case MarketCategory.Alcohol:

                    var userAlcohol = await _inventoryService.GetUserAlcohol((long) context.User.Id, itemId);
                    userAmount = userAlcohol.Amount;

                    break;
                case MarketCategory.Drink:

                    var userDrink = await _inventoryService.GetUserDrink((long) context.User.Id, itemId);
                    userAmount = userDrink.Amount;

                    break;
                case MarketCategory.Food:

                    var userFood = await _inventoryService.GetUserFood((long) context.User.Id, itemId);
                    userAmount = userFood.Amount;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем заявку
            var request = await _marketService.GetMarketUserRequest((long) context.User.Id, category, itemId);

            // проверяем не выставлял ли пользователь уже заявку на этот товар
            if (request != null)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketRequestAlready.Parse(
                    request.Id, emotes.GetEmoteOrBlank(itemName), _local.Localize(category, itemId))));
            }
            // проверяем есть ли у пользователя количество товара которое он хочет выставить
            else if (userAmount < amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketSellRequestNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(itemName), _local.Localize(category, itemId))));
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

                // проверяем что цена пользователя не меньше минимальной цены
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
                        // отнимаем у пользователя предметы по заявке
                        await _inventoryService.RemoveItemFromUser(
                            (long) context.User.Id, category, itemId, amount);
                        // создаем заявку на рынке
                        await _marketService.AddOrUpdateMarketRequest(
                            (long) context.User.Id, category, itemId, price, amount, true);

                        var embed = new EmbedBuilder()
                            // баннер рынка
                            .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket))
                            // подтверждаем успешное создание заявки
                            .WithDescription(IzumiReplyMessage.MarketSellRequestSuccess.Parse(
                                emotes.GetEmoteOrBlank(itemName), amount, _local.Localize(category, itemId, amount),
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
