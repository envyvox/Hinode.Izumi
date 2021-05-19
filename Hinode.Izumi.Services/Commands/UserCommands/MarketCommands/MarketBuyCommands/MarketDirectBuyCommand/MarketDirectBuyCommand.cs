using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.DrinkService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.MarketService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketDirectBuyCommand
{
    [InjectableService]
    public class MarketDirectBuyCommand : IMarketDirectBuyCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IMarketService _marketService;
        private readonly IInventoryService _inventoryService;
        private readonly IMasteryService _masteryService;
        private readonly ICalculationService _calc;
        private readonly ILocalizationService _local;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IGatheringService _gatheringService;
        private readonly ICraftingService _craftingService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly IFoodService _foodService;

        public MarketDirectBuyCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IMarketService marketService, IInventoryService inventoryService, IMasteryService masteryService,
            ICalculationService calc, ILocalizationService local, IUserService userService, IImageService imageService,
            IStatisticService statisticService, IAchievementService achievementService,
            IDiscordGuildService discordGuildService, IGatheringService gatheringService,
            ICraftingService craftingService, IAlcoholService alcoholService, IDrinkService drinkService,
            IFoodService foodService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _marketService = marketService;
            _inventoryService = inventoryService;
            _masteryService = masteryService;
            _calc = calc;
            _local = local;
            _userService = userService;
            _imageService = imageService;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _discordGuildService = discordGuildService;
            _gatheringService = gatheringService;
            _craftingService = craftingService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _foodService = foodService;
        }

        public async Task Execute(SocketCommandContext context, long requestId, long amount = 1)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем заявку
            var request = await _marketService.GetMarketRequest(requestId);
            // получаем валюту пользователя
            var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);

            // проверяем что пользователь не пытается купить у самого себя
            if ((long) context.User.Id == request.UserId)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketBuyYourself.Parse()));
            }
            // проверяем что пользователь не пытается купить больше, чем есть в заявке
            else if (amount > request.Amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketBuyDirectWrongAmount.Parse()));
            }
            // проверяем что у пользователя хватит денег на оплату заявки
            else if (userCurrency.Amount < request.Price * amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketBuyDirectNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString()))));
            }
            else
            {
                // получаем пользователя
                var user = await _userService.GetUser((long) context.User.Id);
                // получаем мастерство торговли владельца заявки
                var userMastery = await _masteryService.GetUserMastery(request.UserId, Mastery.Trading);
                // считаем получаемое количество денег владельцем заявки без вычета налога рынка
                var amountBeforeMarketTax = request.Price * amount;
                // считаем получаемое количество денег владельцем заявки после вычета налога рынка
                var amountAfterMarketTax = await _calc.CurrencyAmountAfterMarketTax(
                    (long) Math.Floor(userMastery.Amount), request.Price * amount);
                // считаем сколько конкретно составил налог рынка
                var marketTaxAmount = amountBeforeMarketTax - amountAfterMarketTax;

                // отнимаем у пользователя деньги на оплату заявки
                await _inventoryService.RemoveItemFromUser(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                    request.Price * amount);
                // добавляем владельцу заявки деньги по заявке
                await _inventoryService.AddItemToUser(
                    request.UserId, InventoryCategory.Currency, Currency.Ien.GetHashCode(), amountAfterMarketTax);
                // обновляем заявку на рынке
                await _marketService.UpdateOrDeleteMarketRequest(
                    request.Category, request.ItemId, request.Amount, amount);
                // добавляем пользователю купленные по заявке предметы
                await _inventoryService.AddItemToUser(
                    (long) context.User.Id, request.Category, request.ItemId, amount);
                // добавляем пользователю статистку покупок на рынке
                await _statisticService.AddStatisticToUser((long) context.User.Id, Statistic.MarketBuy);
                // проверяем выполнил ли пользователь достижения
                await _achievementService.CheckAchievement((long) context.User.Id, new[]
                {
                    Achievement.FirstMarketDeal,
                    Achievement.Market50Buy,
                    Achievement.Market333Buy
                });

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
                    .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket))
                    // подверждаем успешную покупку по заявке
                    .WithDescription(IzumiReplyMessage.MarketBuyDirectSuccess.Parse(
                        emotes.GetEmoteOrBlank(itemName), amount,
                        _local.Localize(request.Category, request.ItemId, amount),
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), request.Price * amount,
                        _local.Localize(Currency.Ien.ToString(), request.Price * amount)));

                await _discordEmbedService.SendEmbed(context.User, embed);

                var notify = new EmbedBuilder()
                    // баннер рынка
                    .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket))
                    // оповещаем владельца заявки о том, что у него купили товар
                    .WithDescription(IzumiReplyMessage.MarketBuyNotify.Parse(
                        emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name,
                        emotes.GetEmoteOrBlank(itemName), amount,
                        _local.Localize(request.Category, request.ItemId, amount),
                        request.Id, emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amountAfterMarketTax,
                        _local.Localize(Currency.Ien.ToString(), amountAfterMarketTax),
                        marketTaxAmount, _local.Localize(Currency.Ien.ToString(), marketTaxAmount)));

                await _discordEmbedService.SendEmbed(await _discordGuildService.GetSocketUser(request.UserId), notify);
                await Task.CompletedTask;
            }
        }
    }
}
