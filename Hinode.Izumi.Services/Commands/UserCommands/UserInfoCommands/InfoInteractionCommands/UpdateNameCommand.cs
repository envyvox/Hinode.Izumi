using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.RpgServices.CertificateService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.InfoInteractionCommands
{
    [CommandCategory(CommandCategory.UserInfo, CommandCategory.UserInfoInteraction)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UpdateNameCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IUserService _userService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IEmoteService _emoteService;
        private readonly ICertificateService _certificateService;

        public UpdateNameCommand(IDiscordEmbedService discordEmbedService, IUserService userService,
            IDiscordGuildService discordGuildService, IEmoteService emoteService,
            ICertificateService certificateService)
        {
            _discordEmbedService = discordEmbedService;
            _userService = userService;
            _discordGuildService = discordGuildService;
            _emoteService = emoteService;
            _certificateService = certificateService;
        }

        [Command("переименоваться"), Alias("rename")]
        [Summary("Изменить игровое имя на новое")]
        [CommandUsage("!переименоваться Вино из бананов")]
        public async Task UpdateNameTask(
            [Summary("Новое игровое имя")] [Remainder] string name)
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();

            // проверяем что у пользователя есть необходимый сертификат
            await _certificateService.GetUserCertificate(
                (long) Context.User.Id, Certificate.Rename.GetHashCode());

            // проверяем что новое игровое имя валидно
            if (!StringExtensions.CheckValid(name))
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UsernameNotValid.Parse(name)));
            }
            else
            {
                // проверяем не занято ли желаемое игровое имя
                var usernameTaken = await _userService.CheckUser(name);
                if (usernameTaken)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UsernameTaken.Parse(name)));
                }
                else
                {
                    // обновляем игровое имя в базе
                    await _userService.UpdateUserName((long) Context.User.Id, name);
                    // переименовываем пользователя в дискорде
                    await _discordGuildService.Rename((long) Context.User.Id, name);
                    // забираем сертификат
                    await _certificateService.RemoveCertificateFromUser(
                        (long) Context.User.Id, Certificate.Rename.GetHashCode());

                    var embed = new EmbedBuilder()
                        // подвертждаем что смена игрового имени прошла успешно
                        .WithDescription(
                            IzumiReplyMessage.RenameSuccess.Parse(name) +
                            IzumiReplyMessage.CertRemoved.Parse(
                                emotes.GetEmoteOrBlank("Certificate"),
                                Certificate.Rename.Localize().ToLower()));

                    await _discordEmbedService.SendEmbed(Context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
