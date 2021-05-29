using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.ReactionRemoved
{
    [InjectableService]
    public class ReactionRemoved : IReactionRemoved
    {
        private readonly IMediator _mediator;

        public ReactionRemoved(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(Cacheable<IUserMessage, ulong> message,
            ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction)
        {
            // игнорируем реакции поставленные ботом
            if (socketReaction.User.Value.IsBot) return;

            // получаем каналы доски сообщества
            var communityDescChannels = await _mediator.Send(new GetCommunityDescChannelsQuery());

            // если сообщение реакции находится в канале доски сообщества
            if (communityDescChannels.Contains(socketMessageChannel.Id))
            {
                // если название реакции не соответствует названиям голосования - игнорируем
                if (socketReaction.Emote.Name != "Like" &&
                    socketReaction.Emote.Name != "Dislike") return;

                // получаем сообщение из базы
                var contentMessage = await _mediator.Send(new GetContentMessageQuery(
                    (long) socketMessageChannel.Id, (long) socketReaction.MessageId));
                // отключаем реакцию пользователя в базе
                await _mediator.Send(new DeactivateUserVoteCommand(
                    (long) socketReaction.UserId, contentMessage.Id,
                    socketReaction.Emote.Name == "Like" ? Vote.Like : Vote.Dislike));
            }
        }
    }
}
