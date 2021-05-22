using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordRoleService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Handlers
{
    public class CheckContentMessageAuthorLikesHandler : IRequestHandler<CheckContentMessageAuthorLikesCommand>
    {
        private readonly IMediator _mediator;

        public CheckContentMessageAuthorLikesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckContentMessageAuthorLikesCommand request,
            CancellationToken cancellationToken)
        {
            // получаем сообщение
            var contentMessage = await _mediator.Send(new GetContentMessageByIdQuery(
                request.ContentMessageId), cancellationToken);
            // получаем количество собранных автором лайков
            var authorLikes = await _mediator.Send(new GetContentAuthorVotesQuery(
                contentMessage.UserId, Vote.Like), cancellationToken);

            // если автор собрал количество кратное 500
            if (authorLikes % 500 == 0)
            {
                // получаем иконки из базы
                var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
                // получаем роли сервера
                var roles = await _mediator.Send(new GetDiscordRolesQuery(), cancellationToken);

                // добавляем ему роль на сервере
                await _mediator.Send(new AddDiscordRoleToUserCommand
                    (contentMessage.UserId, DiscordRole.ContentProvider), cancellationToken);
                // добавляем полученную роль в базу
                await _mediator.Send(new AddDiscordRoleToUserToDbCommand(
                        contentMessage.UserId, roles[DiscordRole.ContentProvider].Id, DateTimeOffset.Now.AddDays(30)),
                    cancellationToken);

                var embed = new EmbedBuilder()
                    .WithAuthor(IzumiReplyMessage.CommunityDescAuthor.Parse())
                    // уведомляем о получении роли
                    .WithDescription(IzumiReplyMessage.CommunityDescAuthorGotRole.Parse(
                        emotes.GetEmoteOrBlank("Like"), DiscordRole.ContentProvider.Name()));

                await _mediator.Send(new SendEmbedToUserCommand(
                    await _mediator.Send(new GetDiscordSocketUserQuery(
                        contentMessage.UserId), cancellationToken), embed), cancellationToken);
            }

            return new Unit();
        }
    }
}
