using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands
{
    public record CheckContentMessageDislikesCommand(long ContentMessageId) : IRequest;

    public class CheckContentMessageDislikesHandler : IRequestHandler<CheckContentMessageDislikesCommand>
    {
        private readonly IMediator _mediator;

        public CheckContentMessageDislikesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckContentMessageDislikesCommand request, CancellationToken cancellationToken)
        {
            // получаем дизлайки сообщения
            var messageDislikes = await _mediator.Send(
                new GetContentMessageVotesQuery(request.ContentMessageId, Vote.Dislike), cancellationToken);

            // если дизлайков 5 или больше
            if (messageDislikes.Length >= 5)
            {
                // получаем это сообщение
                var contentMessage = await _mediator.Send(new GetContentMessageByIdQuery(
                    request.ContentMessageId), cancellationToken);
                var message = await _mediator.Send(new GetDiscordUserMessageQuery(
                    contentMessage.ChannelId, contentMessage.MessageId), cancellationToken);

                // получаем иконки из базы
                var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
                var embed = new EmbedBuilder()
                    .WithAuthor(IzumiReplyMessage.CommunityDescAuthor.Parse())
                    // оповещаем о том, что сообщение удалено
                    .WithDescription(IzumiReplyMessage.CommunityDescMessageDeleted.Parse(
                        emotes.GetEmoteOrBlank("Dislike"), message.Channel.Id))
                    // прикрепляем удаленное вложение
                    .WithImageUrl(message.Attachments.First().Url);

                await _mediator.Send(new SendEmbedToUserCommand(
                    await _mediator.Send(new GetDiscordSocketUserQuery(
                        (long) message.Author.Id), cancellationToken), embed), cancellationToken);

                // удаляем сообщение
                await message.DeleteAsync();
            }

            return new Unit();
        }
    }
}
