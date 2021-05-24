using System;
using System.Linq;
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
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserTitleCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UserTitleCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("титулы"), Alias("titles")]
        [Summary("Посмотреть свою коллекцию титулов")]
        public async Task UserTitleTask()
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем титулы пользователя
            var userTitles = await _mediator.Send(new GetUserTitlesQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithDescription(
                    IzumiReplyMessage.UserTitleDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждого титула создаем embed field
            foreach (var title in Enum.GetValues(typeof(Title))
                .Cast<Title>())
            {
                // если у пользователя нет титула - пропускаем
                if (!userTitles.ContainsKey(title)) continue;
                // если у пользователя больше 25 титулов
                if (embed.Fields.Count == 25)
                {
                    embed.WithFooter("Тут отображены только первые 25 ваших титулов.");
                    continue;
                }

                // отображем информацию о титуле
                embed.AddField(
                    $"{emotes.GetEmoteOrBlank("List")} `{title.GetHashCode()}` {emotes.GetEmoteOrBlank(title.Emote())} {title.Localize()}",
                    $"{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
