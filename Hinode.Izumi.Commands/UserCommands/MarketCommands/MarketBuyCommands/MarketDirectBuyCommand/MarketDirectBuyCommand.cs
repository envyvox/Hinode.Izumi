using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.MarketService.Commands;
using Hinode.Izumi.Services.GameServices.MarketService.Queries;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketBuyCommands.MarketDirectBuyCommand
{
    [InjectableService]
    public class MarketDirectBuyCommand : IMarketDirectBuyCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public MarketDirectBuyCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long requestId, long amount = 1)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем заявку
            var request = await _mediator.Send(new GetMarketRequestByIdQuery(requestId));
            // получаем валюту пользователя
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));

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
                var user = await _mediator.Send(new GetUserByIdQuery((long) context.User.Id));
                // получаем мастерство торговли владельца заявки
                var userMastery = await _mediator.Send(new GetUserMasteryQuery(request.UserId, Mastery.Trading));
                // считаем получаемое количество денег владельцем заявки без вычета налога рынка
                var amountBeforeMarketTax = request.Price * amount;
                // считаем получаемое количество денег владельцем заявки после вычета налога рынка
                var amountAfterMarketTax = await _mediator.Send(new GetCurrencyAmountAfterMarketTaxQuery(
                    (long) Math.Floor(userMastery.Amount), request.Price * amount));
                // считаем сколько конкретно составил налог рынка
                var marketTaxAmount = amountBeforeMarketTax - amountAfterMarketTax;

                // отнимаем у пользователя деньги на оплату заявки
                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                    request.Price * amount));
                // добавляем владельцу заявки деньги по заявке
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    request.UserId, InventoryCategory.Currency, Currency.Ien.GetHashCode(), amountAfterMarketTax));
                // обновляем заявку на рынке
                await _mediator.Send(new UpdateOrDeleteMarketRequestCommand(
                    request.Category, request.ItemId, request.Amount, amount));
                // добавляем пользователю купленные по заявке предметы
                await _mediator.Send(new AddItemToUserByMarketCategoryCommand(
                    (long) context.User.Id, request.Category, request.ItemId, amount));
                // добавляем пользователю статистку покупок на рынке
                await _mediator.Send(new AddStatisticToUserCommand((long) context.User.Id, Statistic.MarketBuy));
                // проверяем выполнил ли пользователь достижения
                await _mediator.Send(new CheckAchievementsInUserCommand((long) context.User.Id, new[]
                {
                    Achievement.FirstMarketDeal,
                    Achievement.Market50Buy,
                    Achievement.Market333Buy
                }));

                // получаем название товара
                var itemName = await _mediator.Send(new GetItemNameQuery(
                    request.Category switch
                    {
                        MarketCategory.Gathering => InventoryCategory.Gathering,
                        MarketCategory.Crafting => InventoryCategory.Crafting,
                        MarketCategory.Alcohol => InventoryCategory.Alcohol,
                        MarketCategory.Drink => InventoryCategory.Drink,
                        MarketCategory.Food => InventoryCategory.Food,
                        MarketCategory.Crop => InventoryCategory.Crop,
                        _ => throw new ArgumentOutOfRangeException()
                    }, request.ItemId));

                var embed = new EmbedBuilder()
                    // баннер рынка
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalMarket)))
                    // подверждаем успешную покупку по заявке
                    .WithDescription(IzumiReplyMessage.MarketBuyDirectSuccess.Parse(
                        emotes.GetEmoteOrBlank(itemName), amount,
                        _local.Localize(request.Category, request.ItemId, amount),
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), request.Price * amount,
                        _local.Localize(Currency.Ien.ToString(), request.Price * amount)));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

                var notify = new EmbedBuilder()
                    // баннер рынка
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.LocationCapitalMarket)))
                    // оповещаем владельца заявки о том, что у него купили товар
                    .WithDescription(IzumiReplyMessage.MarketBuyNotify.Parse(
                        emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name,
                        emotes.GetEmoteOrBlank(itemName), amount,
                        _local.Localize(request.Category, request.ItemId, amount),
                        request.Id, emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amountAfterMarketTax,
                        _local.Localize(Currency.Ien.ToString(), amountAfterMarketTax),
                        marketTaxAmount, _local.Localize(Currency.Ien.ToString(), marketTaxAmount)));

                await _mediator.Send(new SendEmbedToUserCommand(
                    await _mediator.Send(new GetDiscordSocketUserQuery(request.UserId)), notify));
                await Task.CompletedTask;
            }
        }
    }
}
