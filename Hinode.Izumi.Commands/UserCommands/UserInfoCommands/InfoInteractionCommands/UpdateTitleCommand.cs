using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.InfoInteractionCommands
{
    [CommandCategory(CommandCategory.UserInfo, CommandCategory.UserInfoInteraction)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UpdateTitleCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UpdateTitleCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("титул"), Alias("title")]
        [Summary("Обновить текущий титул на указанный")]
        [CommandUsage("!титул 1", "!титул 5")]
        public async Task UpdateTitleTask(
            [Summary("Номер титула")] Title title)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем пользователя из базы
            var user = await _mediator.Send(new GetUserByIdQuery((long) Context.User.Id));
            // получаем титулы пользователя
            var userTitles = await _mediator.Send(new GetUserTitlesQuery((long) Context.User.Id));

            // проверяем есть и у пользователя титул, который он хочет установить
            if (!userTitles.ContainsKey(title))
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserUpdateTitleDontHave.Parse(
                    emotes.GetEmoteOrBlank(title.Emote()), title.Localize())));
            }
            // проверяем не выбран ли у него указанный титул как активный
            else if (user.Title == title)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserUpdateTitleAlready.Parse(
                    emotes.GetEmoteOrBlank(title.Emote()), title.Localize())));
            }
            else
            {
                // меняем титул пользователю
                await _mediator.Send(new UpdateUserTitleCommand((long) Context.User.Id, title));

                var embed = new EmbedBuilder()
                    // подверждаем что смена титула прошла успешно
                    .WithDescription(IzumiReplyMessage.UserUpdateTitleSuccess.Parse(
                        emotes.GetEmoteOrBlank(title.Emote()), title.Localize()));

                await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
