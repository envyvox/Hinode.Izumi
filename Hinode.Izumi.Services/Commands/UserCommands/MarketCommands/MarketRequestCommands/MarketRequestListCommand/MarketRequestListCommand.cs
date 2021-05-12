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
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.MarketService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketRequestCommands.MarketRequestListCommand
{
    [InjectableService]
    public class MarketRequestListCommand : IMarketRequestListCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IMarketService _marketService;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;
        private readonly IGatheringService _gatheringService;
        private readonly ICraftingService _craftingService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly IFoodService _foodService;

        public MarketRequestListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IMarketService marketService, ILocalizationService local, IImageService imageService,
            IGatheringService gatheringService, ICraftingService craftingService, IAlcoholService alcoholService,
            IDrinkService drinkService, IFoodService foodService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _marketService = marketService;
            _local = local;
            _imageService = imageService;
            _gatheringService = gatheringService;
            _craftingService = craftingService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _foodService = foodService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем заявки пользователя на рынке
            var requests = await _marketService.GetMarketUserRequest((long) context.User.Id);

            var embed = new EmbedBuilder()
                // баннер рынка
                .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket))
                // рассказываем как отменять заявки
                .WithDescription(
                    IzumiReplyMessage.MarketRequestListDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждой заявки создаем embed field
            foreach (var request in requests)
            {
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

                // выводим информацию о заявке
                embed.AddField(IzumiReplyMessage.MarketUserRequestFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), request.Id, request.Selling ? "Продаете" : "Покупаете",
                        emotes.GetEmoteOrBlank(itemName), _local.Localize(request.Category, request.ItemId)),
                    IzumiReplyMessage.MarketRequestInfo.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), request.Price,
                        _local.Localize(Currency.Ien.ToString(), request.Price), request.Amount) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");
            }

            if (embed.Fields.Count < 1)
            {
                embed.AddField(IzumiReplyMessage.MarketUserRequestListNullFieldName.Parse(),
                    IzumiReplyMessage.MarketUserRequestListNullFieldDesc.Parse());
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
