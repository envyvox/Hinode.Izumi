using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteAcceptCommand
{
    [InjectableService]
    public class FamilyInviteAcceptCommand : IFamilyInviteAcceptCommand
    {
        private readonly IMediator _mediator;

        public FamilyInviteAcceptCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, long inviteId)
        {
            // получаем приглашение в семью
            var invite = await _mediator.Send(new GetFamilyInviteByIdQuery(inviteId));
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery((long) context.User.Id));

            if (hasFamily)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyAlready.Parse()));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _mediator.Send(new GetEmotesQuery());
                // получаем пользователя
                var user = await _mediator.Send(new GetUserByIdQuery((long) context.User.Id));
                // получаем семью, в которую пользователя пригласили
                var family = await _mediator.Send(new GetFamilyByIdQuery(invite.FamilyId));
                // получаем владельца этой семьи
                var familyOwner = await _mediator.Send(new GetFamilyOwnerQuery(family.Id));

                // удаляем приглашение
                await _mediator.Send(new DeleteFamilyInviteCommand(inviteId));
                // добавляем пользователя в семью
                await _mediator.Send(new AddUserToFamilyByFamilyIdCommand((long) context.User.Id, invite.FamilyId));

                var embed = new EmbedBuilder()
                    // подтверждаем что пользователь успешно принял приглашение
                    .WithDescription(IzumiReplyMessage.FamilyInviteAcceptSuccess.Parse(family.Name));

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

                var notify = new EmbedBuilder()
                    // оповещаем владельца семьи что пользователь принял приглашение
                    .WithDescription(IzumiReplyMessage.FamilyInviteAcceptSuccessNotify.Parse(
                        emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name));

                await _mediator.Send(new SendEmbedToUserCommand(
                    await _mediator.Send(new GetDiscordSocketUserQuery(familyOwner.Id)), notify));
                await Task.CompletedTask;
            }
        }
    }
}
