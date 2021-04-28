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
using Hinode.Izumi.Services.RpgServices.UserService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketCheckTopBuyingRequestsCommand
{
    [InjectableService]
    public class MarketCheckTopBuyingRequestsCommand : IMarketCheckTopBuyingRequestsCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _local;
        private readonly IMarketService _marketService;
        private readonly IUserService _userService;
        private readonly IGatheringService _gatheringService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly IFoodService _foodService;
        private readonly ICraftingService _craftingService;

        public MarketCheckTopBuyingRequestsCommand(IDiscordEmbedService discordEmbedService,
            IEmoteService emoteService, IImageService imageService, ILocalizationService local,
            IMarketService marketService, IUserService userService, IGatheringService gatheringService,
            IAlcoholService alcoholService, IDrinkService drinkService, IFoodService foodService,
            ICraftingService craftingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
            _local = local;
            _marketService = marketService;
            _userService = userService;
            _gatheringService = gatheringService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _foodService = foodService;
            _craftingService = craftingService;
        }

        public async Task Execute(SocketCommandContext context, MarketCategory category, string pattern = null)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем топ 5 заявок в этой категории
            var requests = await _marketService.GetMarketRequest(category, false, pattern);

            var embed = new EmbedBuilder()
                // баннер рынка
                .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket))
                // рассказываем как продавать на рынке
                .WithDescription(
                    IzumiReplyMessage.MarketSellDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждой заявки создаем embed field
            foreach (var request in requests)
            {
                // получаем вледельца заявки
                var user = await _userService.GetUser(request.UserId);

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
                embed.AddField(IzumiReplyMessage.MarketSellFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), request.Id, emotes.GetEmoteOrBlank(user.Title.Emote()),
                        user.Title.Localize(), user.Name, emotes.GetEmoteOrBlank(itemName), _local.Localize(itemName)),
                    IzumiReplyMessage.MarketRequestInfo.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), request.Price,
                        _local.Localize(Currency.Ien.ToString(), request.Price), request.Amount) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");
            }

            if (embed.Fields.Count < 1)
            {
                embed.AddField(IzumiReplyMessage.MarketRequestListNullFieldName.Parse(
                        emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.MarketRequestListNullFieldDesc.Parse());
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
