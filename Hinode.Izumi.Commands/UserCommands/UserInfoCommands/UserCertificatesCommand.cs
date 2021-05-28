using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CertificateService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireRegistry]
    public class UserCertificatesCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UserCertificatesCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("сертификаты"), Alias("certificates")]
        [Summary("Посмотреть приобретенные сертификаты")]
        public async Task UserCertificateTask()
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем сертификаты пользователя
            var userCerts = await _mediator.Send(new GetUserCertificatesQuery((long) Context.User.Id));

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

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
