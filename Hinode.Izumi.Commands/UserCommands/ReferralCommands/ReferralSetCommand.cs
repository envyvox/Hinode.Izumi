using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.Options;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ReferralService.Commands;
using Hinode.Izumi.Services.GameServices.ReferralService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hinode.Izumi.Commands.UserCommands.ReferralCommands
{
    [CommandCategory(CommandCategory.Referral)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class ReferralSetCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly IOptions<DiscordOptions> _options;

        public ReferralSetCommand(IMediator mediator, ILocalizationService local, IOptions<DiscordOptions> options)
        {
            _mediator = mediator;
            _local = local;
            _options = options;
        }

        [Command("пригласил"), Alias("пригласила", "invited")]
        [Summary("Указать пригласившего пользователя")]
        [CommandUsage("!пригласил Холли", "!пригласила Рыбка")]
        public async Task ReferralSetTask(
            [Summary("Игровое имя")] [Remainder] string referralName)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем пригласившего пользователя
            var tUser = await _mediator.Send(new GetUserByNamePatternQuery(referralName));
            // проверяем есть ли у пользователя реферрер
            var hasReferrer = await _mediator.Send(new CheckUserHasReferrerQuery((long) Context.User.Id));

            // проверяем что пользователь не указал самого себя
            if ((long) Context.User.Id == tUser.Id)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ReferralSetYourself.Parse()));
            }
            // проверяем что пользователь не указал Изуми
            else if ((long) _options.Value.BotId == tUser.Id)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ReferralSetIzumi.Parse()));
            }
            // проверяем что пользователь не указывал пригласившего
            else if (hasReferrer)
            {
                // получаем пригласившего пользователя из базы
                var rUser = await _mediator.Send(new GetUserReferrerQuery((long) Context.User.Id));
                await Task.FromException(new Exception(IzumiReplyMessage.ReferralSetAlready.Parse(
                    emotes.GetEmoteOrBlank(rUser.Title.Emote()), rUser.Title.Localize(), rUser.Name)));
            }
            else
            {
                // добавляем информацию о приглашении
                await _mediator.Send(new CreateUserReferrerCommand((long) Context.User.Id, tUser.Id));

                var embed = new EmbedBuilder()
                    // подтверждаем что информация о приглашении добавлена
                    .WithDescription(IzumiReplyMessage.ReferralSetSuccess.Parse(
                        emotes.GetEmoteOrBlank(tUser.Title.Emote()), tUser.Title.Localize(), tUser.Name))
                    // бонус реферальной системы
                    .AddField(IzumiReplyMessage.ReferralRewardFieldName.Parse(),
                        $"{emotes.GetEmoteOrBlank(Box.Capital.Emote())} {_local.Localize(Box.Capital.ToString())}");

                await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
