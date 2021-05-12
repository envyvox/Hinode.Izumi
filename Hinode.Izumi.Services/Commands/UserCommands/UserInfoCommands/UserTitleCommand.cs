using System;
using System.Linq;
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

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserTitleCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IUserService _userService;

        public UserTitleCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IUserService userService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _userService = userService;
        }

        [Command("титулы"), Alias("titles")]
        [Summary("Посмотреть свою коллекцию титулов")]
        public async Task UserTitleTask()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем титулы пользователя
            var userTitles = await _userService.GetUserTitle((long) Context.User.Id);

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

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
