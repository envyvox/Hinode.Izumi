using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyUpdateDescriptionCommand
{
    [InjectableService]
    public class FamilyUpdateDescriptionCommand : IFamilyUpdateDescriptionCommand
    {
        private readonly IMediator _mediator;

        public FamilyUpdateDescriptionCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, string newDescription)
        {
            // получаем пользователя в семье
            var userFamily = await _mediator.Send(new GetUserFamilyQuery((long) context.User.Id));

            // проверяем что пользователь является главой семьи
            if (userFamily.Status != UserInFamilyStatus.Head)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireHead.Parse()));
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
                // проверяем что новое описание семьи не длинее максимального значения
                else if (newDescription.Length > 1024)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyUpdateDescriptionMaxLength.Parse()));
                }
                else
                {
                    // обновляем информацию о семье
                    await _mediator.Send(new UpdateFamilyDescriptionCommand(userFamily.FamilyId, newDescription));

                    var embed = new EmbedBuilder()
                        // подтверждаем что информация о семье успешно обновлена
                        .WithDescription(IzumiReplyMessage.FamilyUpdateDescriptionSuccess.Parse());

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
