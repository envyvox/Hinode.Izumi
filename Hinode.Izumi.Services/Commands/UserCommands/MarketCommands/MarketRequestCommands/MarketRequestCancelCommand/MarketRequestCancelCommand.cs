using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.DrinkService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.MarketService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketRequestCommands.MarketRequestCancelCommand
{
    [InjectableService]
    public class MarketRequestCancelCommand : IMarketRequestCancelCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IMarketService _marketService;
        private readonly ILocalizationService _local;
        private readonly IInventoryService _inventoryService;
        private readonly IImageService _imageService;
        private readonly IGatheringService _gatheringService;
        private readonly ICraftingService _craftingService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly IFoodService _foodService;

        public MarketRequestCancelCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IMarketService marketService, ILocalizationService local, IInventoryService inventoryService,
            IImageService imageService, IGatheringService gatheringService, ICraftingService craftingService,
            IAlcoholService alcoholService, IDrinkService drinkService, IFoodService foodService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _marketService = marketService;
            _local = local;
            _inventoryService = inventoryService;
            _imageService = imageService;
            _gatheringService = gatheringService;
            _craftingService = craftingService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _foodService = foodService;
        }

        public async Task Execute(SocketCommandContext context, long requestId)
        {
            // получаем заявку
            var request = await _marketService.GetMarketRequest(requestId);

            // проверяем что эта заявка принадлежит пользователю
            if ((long) context.User.Id != request.UserId)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketRequestWrongUser.Parse()));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _emoteService.GetEmotes();
                // получаем название товара
                string itemName;
                switch (request.Category)
                {
                    case MarketCategory.Gathering:

                        var gathering = await _gatheringService.GetGathering(request.ItemId);
                        itemName = gathering.Name;

                        break;
                    case MarketCategory.Crafting:

                        var crafting = await _craftingService.GetCrafting(request.ItemId);
                        itemName = crafting.Name;

                        break;
                    case MarketCategory.Alcohol:

                        var alcohol = await _alcoholService.GetAlcohol(request.ItemId);
                        itemName = alcohol.Name;

                        break;
                    case MarketCategory.Drink:

                        var drink = await _drinkService.GetDrink(request.ItemId);
                        itemName = drink.Name;

                        break;
                    case MarketCategory.Food:

                        var food = await _foodService.GetFood(request.ItemId);
                        itemName = food.Name;

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var embed = new EmbedBuilder()
                    // баннер рынка
                    .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket));

                // если эта заявка была на продажу
                if (request.Selling)
                {
                    // возвращаем пользователю предметы, которые он выставлял на продажу
                    await _inventoryService.AddItemToUser(
                        (long) context.User.Id, request.Category, request.ItemId, request.Amount);

                    // подверждаем успешное удаление заявки
                    embed.WithDescription(IzumiReplyMessage.MarketRequestSellCancel.Parse(
                        emotes.GetEmoteOrBlank(itemName), request.Amount,
                        _local.Localize(request.Category, request.ItemId, request.Amount)));
                }
                // если эта заявка была на покупку
                else
                {
                    // возвращаем пользователю деньги, которые он выставил на покупку предметов
                    await _inventoryService.AddItemToUser(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        request.Price * request.Amount);

                    // подверждаем успешное удаление заявки
                    embed.WithDescription(IzumiReplyMessage.MarketRequestBuyCancel.Parse(
                        emotes.GetEmoteOrBlank(itemName), request.Amount,
                        _local.Localize(request.Category, request.ItemId, request.Amount),
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), request.Price * request.Amount,
                        _local.Localize(Currency.Ien.ToString(),
                            request.Price * request.Amount)));
                }

                // удаляем заявку с рынка
                await _marketService.DeleteMarketRequest(requestId);
                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
