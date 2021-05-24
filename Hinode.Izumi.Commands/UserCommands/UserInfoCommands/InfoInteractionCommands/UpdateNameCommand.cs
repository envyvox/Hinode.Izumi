using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CertificateService.Commands;
using Hinode.Izumi.Services.GameServices.CertificateService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.InfoInteractionCommands
{
    [CommandCategory(CommandCategory.UserInfo, CommandCategory.UserInfoInteraction)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UpdateNameCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UpdateNameCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("переименоваться"), Alias("rename")]
        [Summary("Изменить игровое имя на новое")]
        [CommandUsage("!переименоваться Вино из бананов")]
        public async Task UpdateNameTask(
            [Summary("Новое игровое имя")] [Remainder]
            string name)
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());

            // проверяем что у пользователя есть необходимый сертификат
            await _mediator.Send(new GetUserCertificateQuery(
                (long) Context.User.Id, Certificate.Rename.GetHashCode()));

            // проверяем что новое игровое имя валидно
            if (!StringExtensions.CheckValid(name))
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UsernameNotValid.Parse(name)));
            }
            else
            {
                // проверяем не занято ли желаемое игровое имя
                var usernameTaken = await _mediator.Send(new CheckUserByNameQuery(name));
                if (usernameTaken)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UsernameTaken.Parse(name)));
                }
                else
                {
                    // обновляем игровое имя в базе
                    await _mediator.Send(new UpdateUserNameCommand((long) Context.User.Id, name));
                    // переименовываем пользователя в дискорде
                    await _mediator.Send(new RenameDiscordUserCommand((long) Context.User.Id, name));
                    // забираем сертификат
                    await _mediator.Send(new RemoveCertificateFromUserCommand(
                        (long) Context.User.Id, Certificate.Rename.GetHashCode()));

                    var embed = new EmbedBuilder()
                        // подвертждаем что смена игрового имени прошла успешно
                        .WithDescription(
                            IzumiReplyMessage.RenameSuccess.Parse(name) +
                            IzumiReplyMessage.CertRemoved.Parse(
                                emotes.GetEmoteOrBlank("Certificate"),
                                Certificate.Rename.Localize().ToLower()));

                    await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
