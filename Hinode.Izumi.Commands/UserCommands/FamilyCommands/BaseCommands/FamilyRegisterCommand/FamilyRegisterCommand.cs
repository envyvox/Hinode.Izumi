using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CertificateService.Commands;
using Hinode.Izumi.Services.GameServices.CertificateService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyRegisterCommand
{
    [InjectableService]
    public class FamilyRegisterCommand : IFamilyRegisterCommand
    {
        private readonly IMediator _mediator;

        public FamilyRegisterCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, string familyName)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // проверяем есть ли у пользователя сертификат на регистрацию семьи
            await _mediator.Send(new GetUserCertificateQuery(
                (long) context.User.Id, Certificate.FamilyRegistration.GetHashCode()));

            // проверяем состоит ли пользователь в семье
            var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery((long) context.User.Id));

            if (hasFamily)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyAlready.Parse()));
            }
            else
            {
                // проверяем название семьи на валидность
                var checkValid = StringExtensions.CheckValid(familyName);

                if (!checkValid)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyNameNotValid.Parse(familyName)));
                }
                else
                {
                    // проверяем есть ли семья с таким названием
                    var checkFamily = await _mediator.Send(new CheckFamilyWithNameQuery(familyName));

                    if (checkFamily)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.FamilyNameTaken.Parse(familyName)));
                    }
                    else
                    {
                        // создаем семью
                        await _mediator.Send(new CreateFamilyCommand(familyName));
                        // добавляем пользователя в семью
                        await _mediator.Send(new AddUserToFamilyByFamilyNameCommand(
                            (long) context.User.Id, familyName));
                        // отнимаем сертификат у пользователя
                        await _mediator.Send(new RemoveCertificateFromUserCommand(
                            (long) context.User.Id, Certificate.FamilyRegistration.GetHashCode()));

                        var embed = new EmbedBuilder()
                            // подтверждаем успешное создание семьи
                            .WithDescription(
                                IzumiReplyMessage.FamilyRegistrationSuccess.Parse(familyName) +
                                IzumiReplyMessage.CertRemoved.Parse(
                                    emotes.GetEmoteOrBlank("Certificate"),
                                    Certificate.FamilyRegistration.Localize().ToLower()));

                        await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
