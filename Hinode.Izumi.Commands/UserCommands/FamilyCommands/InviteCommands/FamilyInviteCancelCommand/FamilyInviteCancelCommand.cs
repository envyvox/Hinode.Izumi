using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteCancelCommand
{
    [InjectableService]
    public class FamilyInviteCancelCommand : IFamilyInviteCancelCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public FamilyInviteCancelCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long inviteId)
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
                // получаем приглашение
                var invite = await _mediator.Send(new GetFamilyInviteByIdQuery(inviteId));
                // получаем семью
                var family = await _mediator.Send(new GetFamilyByIdQuery(invite.FamilyId));

                // удаляем приглашение в семью
                await _mediator.Send(new DeleteFamilyInviteCommand(inviteId));

                var embed = new EmbedBuilder()
                    // подтверждаем что приглашение успешно отменено
                    .WithDescription(IzumiReplyMessage.FamilyInviteCancelSuccess.Parse(invite.Id));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

                var notify = new EmbedBuilder()
                    // оповещаем пользователя что его приглашение в семью отменено
                    .WithDescription(IzumiReplyMessage.FamilyInviteCancelSuccessNotify.Parse(family.Name));

                await _mediator.Send(new SendEmbedToUserCommand(
                    await _mediator.Send(new GetDiscordSocketUserQuery(invite.UserId)), notify));
                await Task.CompletedTask;
            }
        }
    }
}
