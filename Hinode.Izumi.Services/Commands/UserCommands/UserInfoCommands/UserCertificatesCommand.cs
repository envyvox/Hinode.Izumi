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
using Hinode.Izumi.Services.RpgServices.CertificateService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserCertificatesCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICertificateService _certificateService;

        public UserCertificatesCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICertificateService certificateService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _certificateService = certificateService;
        }

        [Command("сертификаты"), Alias("certificates")]
        [Summary("Посмотреть приобретенные сертификаты")]
        public async Task UserCertificateTask()
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем сертификаты пользователя
            var userCerts = await _certificateService.GetUserCertificate((long) Context.User.Id);

            var embed = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.UserCertsDesc.Parse())
                // рассказываем что сертификаты после использования будут изъяты
                .WithFooter(IzumiReplyMessage.UserCertsFooter.Parse());

            // для каждого сертификата создаем embed field
            foreach (var (_, certificate) in userCerts)
            {
                embed.AddField(IzumiReplyMessage.UserCertsFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), emotes.GetEmoteOrBlank("Certificate"),
                        // название сертификата
                        certificate.Type.Localize()),
                    // описание и как использовать
                    certificate.Type.Description());
            }

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
