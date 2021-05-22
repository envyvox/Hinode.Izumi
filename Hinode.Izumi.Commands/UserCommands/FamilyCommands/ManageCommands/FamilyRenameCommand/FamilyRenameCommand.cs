using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CertificateService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyRenameCommand
{
    [InjectableService]
    public class FamilyRenameCommand : IFamilyRenameCommand
    {
        private readonly IMediator _mediator;

        public FamilyRenameCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, string newFamilyName)
        {
            // проверяем есть ли у пользователя сертификат
            await _mediator.Send(new GetUserCertificateQuery(
                (long) context.User.Id, Certificate.FamilyRename.GetHashCode()));
            // получаем пользователя в семье
            var userFamily = await _mediator.Send(new GetUserFamilyQuery((long) context.User.Id));

            // проверяем что пользователь является главой семьи
            if (userFamily.Status != UserInFamilyStatus.Head)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireHead.Parse()));
            }
            else
            {
                // проверяем новое имя на валидность
                var checkValid = StringExtensions.CheckValid(newFamilyName);

                if (!checkValid)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyNameNotValid.Parse(newFamilyName)));
                }
                else
                {
                    // проверяем что с желаемым именем нет семьи
                    var familyWithName = await _mediator.Send(new CheckFamilyWithNameQuery(newFamilyName));

                    if (familyWithName)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.FamilyNameTaken.Parse(newFamilyName)));
                    }
                    else
                    {
                        // получаем семью
                        var family = await _mediator.Send(new GetFamilyByIdQuery(userFamily.FamilyId));

                        // проверяем что семья прошла этап регистрации
                        if (family.Status == FamilyStatus.Registration)
                        {
                            await Task.FromException(new Exception(IzumiReplyMessage.FamilyStatusRegistration.Parse()));
                        }
                        else
                        {
                            // обновляем название семьи
                            await _mediator.Send(new UpdateFamilyNameCommand(userFamily.FamilyId, newFamilyName));

                            // получаем иконки из базы
                            var emotes = await _mediator.Send(new GetEmotesQuery());
                            var embed = new EmbedBuilder()
                                .WithDescription(
                                    // подтверждаем что семья успешно переименована
                                    IzumiReplyMessage.FamilyRenameSuccess.Parse(newFamilyName) +
                                    // говорим о том, что сертификат был изъят
                                    IzumiReplyMessage.CertRemoved.Parse(
                                        emotes.GetEmoteOrBlank("Certificate"),
                                        Certificate.FamilyRename.Localize().ToLower()));

                            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                            await Task.CompletedTask;
                        }
                    }
                }
            }
        }
    }
}
