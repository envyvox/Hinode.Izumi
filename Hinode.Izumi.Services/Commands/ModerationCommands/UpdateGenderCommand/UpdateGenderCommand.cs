using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.ModerationCommands.UpdateGenderCommand
{
    [InjectableService]
    public class UpdateGenderCommand : IUpdateGenderCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IUserService _userService;
        private readonly IDiscordGuildService _discordGuildService;

        public UpdateGenderCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IUserService userService, IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _userService = userService;
            _discordGuildService = discordGuildService;
        }

        public async Task Execute(SocketCommandContext context, long userId, Gender gender)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем пользователя в дискорде
            var socketUser = await _discordGuildService.GetSocketUser(userId);

            var embed = new EmbedBuilder()
                // подверждаем обновление пола
                .WithDescription(IzumiReplyMessage.ModGenderDesc.Parse(
                    socketUser.Mention, emotes.GetEmoteOrBlank(gender.Emote()), gender.Localize()));

            var notifyEmbed = new EmbedBuilder()
                // оповещаем пользователя что его пол обновлен
                .WithDescription(IzumiReplyMessage.ModGenderNotifyDesc.Parse(
                    emotes.GetEmoteOrBlank(gender.Emote()), gender.Localize()));

            // обновляем пол пользователя в базе
            await _userService.UpdateUserGender(userId, gender);
            await _discordEmbedService.SendEmbed(context.Channel, embed);
            await _discordEmbedService.SendEmbed(socketUser, notifyEmbed);
        }
    }
}
