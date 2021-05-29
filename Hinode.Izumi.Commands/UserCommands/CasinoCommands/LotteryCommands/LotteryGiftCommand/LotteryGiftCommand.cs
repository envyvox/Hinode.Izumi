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
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
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
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryGiftCommand
{
    [InjectableService]
    public class LotteryGiftCommand : ILotteryGiftCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public LotteryGiftCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, string namePattern)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // находим пользователя которому нужно сделать подарок
            var target = await _mediator.Send(new GetUserByNamePatternQuery(namePattern));

            // проверяем не является ли цель этим же пользователем
            if (target.Id == (long) context.User.Id)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.LotteryGiftYourself.Parse(
                    emotes.GetEmoteOrBlank("LotteryTicket"))));
            }
            else
            {
                // получаем валюту пользователя
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(
                    (long) context.User.Id, Currency.Ien));
                // получаем стоимость лотерейного билета
                var lotteryPrice = await _mediator.Send(new GetPropertyValueQuery(
                    Property.LotteryPrice));
                // получаем стоимость отправки лотерейного билета в подарок
                var lotteryDeliveryPrice = await _mediator.Send(new GetPropertyValueQuery(
                    Property.LotteryDeliveryPrice));
                // проверяем есть ли у цели эффект лотереи
                var targetHasLottery = await _mediator.Send(new CheckUserHasEffectQuery(
                    target.Id, Effect.Lottery));

                if (targetHasLottery)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.LotteryGiftAlreadyHave.Parse(
                        emotes.GetEmoteOrBlank(target.Title.Emote()), target.Title.Localize(),
                        target.Name, emotes.GetEmoteOrBlank("LotteryTicket"))));
                }
                // проверяем хватает ли денег пользователю для оплаты подарка лотерейного билета
                else if (userCurrency.Amount < lotteryPrice + lotteryDeliveryPrice)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.LotteryGiftNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        _local.Localize(Currency.Ien.ToString()))));
                }
                else
                {
                    // отнимаем у пользователя деньги за подарок лотерейного билета
                    await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        lotteryPrice + lotteryDeliveryPrice));
                    // добавляем цели эффект лотереи
                    await _mediator.Send(new AddEffectToUserCommand(
                        target.Id, EffectCategory.Lottery, Effect.Lottery));
                    // побавляем пользователю статистику подаренных лотерейных билетов
                    await _mediator.Send(new AddStatisticToUserCommand(
                        (long) context.User.Id, Statistic.CasinoLotteryGift));
                    // проверяем не выполнил ли пользователь достижение
                    await _mediator.Send(new CheckAchievementInUserCommand(
                        (long) context.User.Id, Achievement.Casino20LotteryGift));

                    var embed = new EmbedBuilder()
                        // подтвеждаем успешную отправку подарка лотерейного билета
                        .WithDescription(IzumiReplyMessage.LotteryGiftSuccess.Parse(
                            emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(target.Title.Emote()),
                            target.Title.Localize(), target.Name));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

                    // получаем пользователя который делает подарок
                    var user = await _mediator.Send(new GetUserByIdQuery((long) context.User.Id));
                    var embedPm = new EmbedBuilder()
                        // оповещаем цель о том, что ей подарили лотерейный билет
                        .WithDescription(IzumiReplyMessage.LotteryGiftSuccessPm.Parse(
                            emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(user.Title.Emote()),
                            user.Title.Localize(), user.Name));

                    await _mediator.Send(new SendEmbedToUserCommand(
                        await _mediator.Send(new GetDiscordSocketUserQuery(target.Id)), embedPm));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
