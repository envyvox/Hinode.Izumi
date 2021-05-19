using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
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

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketDirectSellCommand
{
    [InjectableService]
    public class MarketDirectSellCommand : IMarketDirectSellCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IMarketService _marketService;
        private readonly ILocalizationService _local;
        private readonly ICalculationService _calc;
        private readonly IImageService _imageService;
        private readonly IInventoryService _inventoryService;
        private readonly IMasteryService _masteryService;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly IGatheringService _gatheringService;
        private readonly ICraftingService _craftingService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly IFoodService _foodService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IUserService _userService;

        public MarketDirectSellCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IMarketService marketService, ILocalizationService local, ICalculationService calc,
            IImageService imageService, IInventoryService inventoryService, IMasteryService masteryService,
            IStatisticService statisticService, IAchievementService achievementService,
            IGatheringService gatheringService, ICraftingService craftingService, IAlcoholService alcoholService,
            IDrinkService drinkService, IFoodService foodService, IDiscordGuildService discordGuildService,
            IUserService userService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _marketService = marketService;
            _local = local;
            _calc = calc;
            _imageService = imageService;
            _inventoryService = inventoryService;
            _masteryService = masteryService;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _gatheringService = gatheringService;
            _craftingService = craftingService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _foodService = foodService;
            _discordGuildService = discordGuildService;
            _userService = userService;
        }

        public async Task Execute(SocketCommandContext context, long requestId, long amount = 1)
        {
            // получаем заявку
            var request = await _marketService.GetMarketRequest(requestId);

            // проверяем что пользователь не пытается продать самому себе
            if ((long) context.User.Id == request.UserId)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketSellYourself.Parse()));
            }
            // проверяем что пользователь не пытается продать больше, чем есть в заявке
            else if (amount > request.Amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.MarketSellDirectWrongAmount.Parse()));
            }
            else
            {
                // получаем количество товара у пользователя
                long userAmount;
                switch (request.Category)
                {
                    case MarketCategory.Gathering:

                        var userGathering =
                            await _inventoryService.GetUserGathering((long) context.User.Id, request.ItemId);
                        userAmount = userGathering.Amount;

                        break;
                    case MarketCategory.Crafting:

                        var userCrafting =
                            await _inventoryService.GetUserCrafting((long) context.User.Id, request.ItemId);
                        userAmount = userCrafting.Amount;

                        break;
                    case MarketCategory.Alcohol:

                        var userAlcohol =
                            await _inventoryService.GetUserAlcohol((long) context.User.Id, request.ItemId);
                        userAmount = userAlcohol.Amount;

                        break;
                    case MarketCategory.Drink:

                        var userDrink = await _inventoryService.GetUserDrink((long) context.User.Id, request.ItemId);
                        userAmount = userDrink.Amount;

                        break;
                    case MarketCategory.Food:

                        var userFood = await _inventoryService.GetUserFood((long) context.User.Id, request.ItemId);
                        userAmount = userFood.Amount;

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // проверяем что у пользователя есть столько товара, сколько он хочет продать
                if (amount > userAmount)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.MarketSellRequestNoCurrency.Parse()));
                }
                else
                {
                    // получаем мастерство торговли пользователя
                    var userMastery = await _masteryService.GetUserMastery((long) context.User.Id, Mastery.Trading);
                    // считаем получаемое количество денег пользователем без вычета налога рынка
                    var amountBeforeMarketTax = request.Price * amount;
                    // считаем получаемое количество денег пользователем после вычета налога рынка
                    var amountAfterMarketTax = await _calc.CurrencyAmountAfterMarketTax(
                        (long) Math.Floor(userMastery.Amount), request.Price * amount);
                    // считаем сколько конкретно составил налог рынка
                    var marketTaxAmount = amountBeforeMarketTax - amountAfterMarketTax;

                    // отнимаем у пользователя продаваемый товар
                    await _inventoryService.RemoveItemFromUser(
                        (long) context.User.Id, request.Category, request.ItemId, amount);
                    // добавляем вледельцу заявки купленный товар
                    await _inventoryService.AddItemToUser(
                        request.UserId, request.Category, request.ItemId, amount);
                    // добавляем пользователю деньги за проданный товар
                    await _inventoryService.AddItemToUser(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        amountAfterMarketTax);
                    // добавляем пользователю статистику проданных на рынке товаров
                    await _statisticService.AddStatisticToUser((long) context.User.Id, Statistic.MarketSell);
                    await _achievementService.CheckAchievement((long) context.User.Id, new[]
                    {
                        Achievement.FirstMarketDeal,
                        Achievement.Market100Sell,
                        Achievement.Market666Sell
                    });

                    // получаем название предмета
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

                    // обновляем заявку на рынке
                    await _marketService.UpdateOrDeleteMarketRequest(
                        request.Category, request.ItemId, request.Amount, amount);
                    // добавляем пользователю мастерство торговли
                    await _masteryService.AddMasteryToUser((long) context.User.Id, Mastery.Trading,
                        await _calc.MasteryXp(MasteryXpProperty.Trading, (long) Math.Floor(userMastery.Amount),
                            amount));

                    // получаем иконки из базы
                    var emotes = await _emoteService.GetEmotes();
                    var embed = new EmbedBuilder()
                        // баннер рынка
                        .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket))
                        // подтверждаем успешную продажу на рынке
                        .WithDescription(IzumiReplyMessage.MarketSellDirectSuccess.Parse(
                            emotes.GetEmoteOrBlank(itemName), amount,
                            _local.Localize(request.Category, request.ItemId, amount),
                            emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amountAfterMarketTax,
                            _local.Localize(Currency.Ien.ToString(), amountAfterMarketTax),
                            marketTaxAmount, _local.Localize(Currency.Ien.ToString(), marketTaxAmount)));

                    await _discordEmbedService.SendEmbed(context.User, embed);

                    // получаем пользователя
                    var user = await _userService.GetUser((long) context.User.Id);
                    var notify = new EmbedBuilder()
                        // баннер рынка
                        .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket))
                        // оповещаем владельца заявки что ему продали товар
                        .WithDescription(IzumiReplyMessage.MarketSellNotify.Parse(
                            emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name,
                            emotes.GetEmoteOrBlank(itemName), amount,
                            _local.Localize(request.Category, request.ItemId, amount), request.Id));

                    await _discordEmbedService.SendEmbed(
                        await _discordGuildService.GetSocketUser(request.UserId), notify);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
