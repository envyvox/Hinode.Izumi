using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.EffectService.Commands;
using Hinode.Izumi.Services.GameServices.EffectService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryBuyCommand
{
    [InjectableService]
    public class LotteryBuyCommand : ILotteryBuyCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public LotteryBuyCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем валюту пользователя
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));
            // получаем стоимость лотерейного билета
            var lotteryPrice = await _mediator.Send(new GetPropertyValueQuery(Property.LotteryPrice));
            // проверяем есть ли у пользователя эффект лотереи
            var hasLottery = await _mediator.Send(new CheckUserHasEffectQuery((long) context.User.Id, Effect.Lottery));

            if (hasLottery)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.LotteryBuyAlready.Parse(
                    emotes.GetEmoteOrBlank("LotteryTicket"))));
            }
            // проверяем хватает ли пользователю денег на покупку лотерейного билета
            else if (userCurrency.Amount < lotteryPrice)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.LotteryBuyNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                    _local.Localize(Currency.Ien.ToString()), emotes.GetEmoteOrBlank("LotteryTicket"))));
            }
            else
            {
                // забираем у пользователя деньги за лотерейный билет
                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), lotteryPrice));
                // добавляем пользователю эффект лотереи
                await _mediator.Send(new AddEffectToUserCommand(
                    (long) context.User.Id, EffectCategory.Lottery, Effect.Lottery));
                // добавляем пользователю статистику купленных лотерейных билетов
                await _mediator.Send(new AddStatisticToUserCommand((long) context.User.Id, Statistic.CasinoLotteryBuy));
                // проверяем не выполнил ли пользователь достижение
                await _mediator.Send(new CheckAchievementsInUserCommand((long) context.User.Id, new[]
                {
                    Achievement.FirstLotteryTicket,
                    Achievement.Casino22LotteryBuy,
                    Achievement.Casino99LotteryBuy
                }));

                var embed = new EmbedBuilder()
                    // подверждаем успешную покупку лотерейного билета
                    .WithDescription(IzumiReplyMessage.LotteryBuySuccess.Parse(
                        emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        lotteryPrice, _local.Localize(Currency.Ien.ToString(), lotteryPrice)));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
