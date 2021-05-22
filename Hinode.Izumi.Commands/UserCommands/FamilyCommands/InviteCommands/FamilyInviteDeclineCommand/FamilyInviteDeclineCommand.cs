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

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteDeclineCommand
{
    [InjectableService]
    public class FamilyInviteDeclineCommand : IFamilyInviteDeclineCommand
    {
        private readonly IMediator _mediator;

        public FamilyInviteDeclineCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, long inviteId)
        {
            // получаем приглашение
            var invite = await _mediator.Send(new GetFamilyInviteByIdQuery(inviteId));
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем пользователя
            var user = await _mediator.Send(new GetUserByIdQuery((long) context.User.Id));
            // получаем семью
            var family = await _mediator.Send(new GetFamilyByIdQuery(invite.FamilyId));

            // удаляем приглашение
            await _mediator.Send(new DeleteFamilyInviteCommand(inviteId));

            var embed = new EmbedBuilder()
                // подтверждаем что приглашение успешно отклонено
                .WithDescription(IzumiReplyMessage.FamilyInviteDeclineSuccess.Parse(family.Name));

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

            // получаем владельца семьи
            var familyOwner = await _mediator.Send(new GetFamilyOwnerQuery(invite.FamilyId));
            var notify = new EmbedBuilder()
                // оповещаем владельца семьи о том, что приглашение было отклонено
                .WithDescription(IzumiReplyMessage.FamilyInviteDeclineSuccessNotify.Parse(
                    emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name));

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(familyOwner.Id)), notify));
            await Task.CompletedTask;
        }
    }
}
