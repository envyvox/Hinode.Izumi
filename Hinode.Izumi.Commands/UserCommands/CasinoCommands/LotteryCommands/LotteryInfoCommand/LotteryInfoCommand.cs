using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.EffectService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryInfoCommand
{
    [InjectableService]
    public class LotteryInfoCommand : ILotteryInfoCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public LotteryInfoCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем список пользователь с эффектом лотереи
            var lotteryUsers = await _mediator.Send(new GetUsersWithEffectQuery(Effect.Lottery));
            // заполняем список для вывода
            var lotteryUsersString = lotteryUsers.Aggregate(string.Empty, (current, user) => current +
                $"{emotes.GetEmoteOrBlank("List")} {emotes.GetEmoteOrBlank(user.Title.Emote())} {user.Title.Localize()} **{user.Name}**\n");

            // получаем стоимость лотерейного билета
            var lotteryPrice = await _mediator.Send(new GetPropertyValueQuery(
                Property.LotteryPrice));
            // получаем награду за победу в лотерее
            var lotteryAward = await _mediator.Send(new GetPropertyValueQuery(
                Property.LotteryAward));
            // получаем стоимость отправки лотерейного билета в подарок
            var lotteryDeliveryPrice = await _mediator.Send(new GetPropertyValueQuery(
                Property.LotteryDeliveryPrice));
            // получаем необходимое количество пользователей для лотереи
            var lotteryRequireUsers = await _mediator.Send(new GetPropertyValueQuery(
                Property.LotteryRequireUsers));

            var embed = new EmbedBuilder()
                // правила участия
                .AddField(IzumiReplyMessage.LotteryInfoRulesFieldName.Parse(),
                    IzumiReplyMessage.LotteryInfoRulesFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        lotteryPrice, _local.Localize(Currency.Ien.ToString(), lotteryPrice),
                        lotteryRequireUsers, lotteryAward, _local.Localize(Currency.Ien.ToString(), lotteryAward)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")

                // служба доставки
                .AddField(IzumiReplyMessage.LotteryGiftInfoFieldName.Parse(),
                    IzumiReplyMessage.LotteryGiftInfoFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        lotteryDeliveryPrice, _local.Localize(Currency.Ien.ToString(), lotteryDeliveryPrice)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")

                // список пользователей с лотереей
                .AddField(IzumiReplyMessage.LotteryInfoCurrentMembersFieldName.Parse(),
                    lotteryUsersString.Length > 0
                        ? lotteryUsersString
                        : IzumiReplyMessage.LotteryInfoCurrentMembersNull.Parse(
                            emotes.GetEmoteOrBlank("LotteryTicket")));

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }
    }
}
