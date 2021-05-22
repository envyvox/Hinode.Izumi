using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CalculationService.Handlers;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.MarketService.Commands;
using Hinode.Izumi.Services.GameServices.MarketService.Queries;
using Hinode.Izumi.Services.GameServices.MasteryService.Commands;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketDirectSellCommand
{
    [InjectableService]
    public class MarketDirectSellCommand : IMarketDirectSellCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public MarketDirectSellCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long requestId, long amount = 1)
        {
            // получаем заявку
            var request = await _mediator.Send(new GetMarketRequestByIdQuery(requestId));

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

                        var userGathering = await _mediator.Send(new GetUserGatheringQuery(
                            (long) context.User.Id, request.ItemId));
                        userAmount = userGathering.Amount;

                        break;
                    case MarketCategory.Crafting:

                        var userCrafting = await _mediator.Send(new GetUserCraftingQuery(
                            (long) context.User.Id, request.ItemId));
                        userAmount = userCrafting.Amount;

                        break;
                    case MarketCategory.Alcohol:

                        var userAlcohol = await _mediator.Send(new GetUserAlcoholQuery(
                            (long) context.User.Id, request.ItemId));
                        userAmount = userAlcohol.Amount;

                        break;
                    case MarketCategory.Drink:

                        var userDrink = await _mediator.Send(new GetUserDrinkQuery(
                            (long) context.User.Id, request.ItemId));
                        userAmount = userDrink.Amount;

                        break;
                    case MarketCategory.Food:

                        var userFood = await _mediator.Send(new GetUserFoodQuery(
                            (long) context.User.Id, request.ItemId));
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
                    var userMastery = await _mediator.Send(new GetUserMasteryQuery(
                        (long) context.User.Id, Mastery.Trading));
                    // считаем получаемое количество денег пользователем без вычета налога рынка
                    var amountBeforeMarketTax = request.Price * amount;
                    // считаем получаемое количество денег пользователем после вычета налога рынка
                    var amountAfterMarketTax = await _mediator.Send(new GetCurrencyAmountAfterMarketTaxQuery(
                        (long) Math.Floor(userMastery.Amount), request.Price * amount));
                    // считаем сколько конкретно составил налог рынка
                    var marketTaxAmount = amountBeforeMarketTax - amountAfterMarketTax;

                    // отнимаем у пользователя продаваемый товар
                    await _mediator.Send(new RemoveItemFromUserByMarketCategoryCommand(
                        (long) context.User.Id, request.Category, request.ItemId, amount));
                    // добавляем вледельцу заявки купленный товар
                    await _mediator.Send(new AddItemToUserByMarketCategoryCommand(
                        request.UserId, request.Category, request.ItemId, amount));
                    // добавляем пользователю деньги за проданный товар
                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        amountAfterMarketTax));
                    // добавляем пользователю статистику проданных на рынке товаров
                    await _mediator.Send(new AddStatisticToUserCommand((long) context.User.Id, Statistic.MarketSell));
                    await _mediator.Send(new CheckAchievementsInUserCommand((long) context.User.Id, new[]
                    {
                        Achievement.FirstMarketDeal,
                        Achievement.Market100Sell,
                        Achievement.Market666Sell
                    }));

                    // получаем название предмета
                    var itemName = await _mediator.Send(new GetItemNameQuery(
                        request.Category switch
                        {
                            MarketCategory.Gathering => InventoryCategory.Gathering,
                            MarketCategory.Crafting => InventoryCategory.Crafting,
                            MarketCategory.Alcohol => InventoryCategory.Alcohol,
                            MarketCategory.Drink => InventoryCategory.Drink,
                            MarketCategory.Food => InventoryCategory.Food,
                            _ => throw new ArgumentOutOfRangeException()
                        }, request.ItemId));

                    // обновляем заявку на рынке
                    await _mediator.Send(new UpdateOrDeleteMarketRequestCommand(
                        request.Category, request.ItemId, request.Amount, amount));
                    // добавляем пользователю мастерство торговли
                    await _mediator.Send(new AddMasteryToUserCommand(
                        (long) context.User.Id, Mastery.Trading, await _mediator.Send(new GetMasteryXpQuery(
                            MasteryXpProperty.Trading, (long) Math.Floor(userMastery.Amount), amount))));

                    // получаем иконки из базы
                    var emotes = await _mediator.Send(new GetEmotesQuery());
                    var embed = new EmbedBuilder()
                        // баннер рынка
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalMarket)))
                        // подтверждаем успешную продажу на рынке
                        .WithDescription(IzumiReplyMessage.MarketSellDirectSuccess.Parse(
                            emotes.GetEmoteOrBlank(itemName), amount,
                            _local.Localize(request.Category, request.ItemId, amount),
                            emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amountAfterMarketTax,
                            _local.Localize(Currency.Ien.ToString(), amountAfterMarketTax),
                            marketTaxAmount, _local.Localize(Currency.Ien.ToString(), marketTaxAmount)));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

                    // получаем пользователя
                    var user = await _mediator.Send(new GetUserByIdQuery((long) context.User.Id));
                    var notify = new EmbedBuilder()
                        // баннер рынка
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalMarket)))
                        // оповещаем владельца заявки что ему продали товар
                        .WithDescription(IzumiReplyMessage.MarketSellNotify.Parse(
                            emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name,
                            emotes.GetEmoteOrBlank(itemName), amount,
                            _local.Localize(request.Category, request.ItemId, amount), request.Id));

                    await _mediator.Send(new SendEmbedToUserCommand(
                        await _mediator.Send(new GetDiscordSocketUserQuery(request.UserId)), notify));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
