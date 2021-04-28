using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.InfoInteractionCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UpdateTitleCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IUserService _userService;

        public UpdateTitleCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IUserService userService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _userService = userService;
        }

        [Command("титул"), Alias("title")]
        public async Task UpdateTitleTask(Title title)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем пользователя из базы
            var user = await _userService.GetUser((long) Context.User.Id);
            // получаем титулы пользователя
            var userTitles = await _userService.GetUserTitle((long) Context.User.Id);

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
                await _userService.UpdateUserTitle((long) Context.User.Id, title);

                var embed = new EmbedBuilder()
                    // подверждаем что смена титула прошла успешно
                    .WithDescription(IzumiReplyMessage.UserUpdateTitleSuccess.Parse(
                        emotes.GetEmoteOrBlank(title.Emote()), title.Localize()));

                await _discordEmbedService.SendEmbed(Context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
