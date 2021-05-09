using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.ReactionRemoved
{
    [InjectableService]
    public class ReactionRemoved : IReactionRemoved
    {
        private readonly IDiscordGuildService _discordGuildService;
        private readonly ICommunityDescService _communityDescService;

        public ReactionRemoved(IDiscordGuildService discordGuildService, ICommunityDescService communityDescService)
        {
            _discordGuildService = discordGuildService;
            _communityDescService = communityDescService;
        }

        public async Task Execute(Cacheable<IUserMessage, ulong> message,
            ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction)
        {
            // игнорируем реакции поставленные ботом
            if (socketReaction.User.Value.IsBot) return;

            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем каналы доски сообщества
            var communityDescChannels = _communityDescService.CommunityDescChannels(channels);

            // если сообщение реакции находится в канале доски сообщества
            if (communityDescChannels.Contains(socketMessageChannel.Id))
            {
                // если название реакции не соответствует названиям голосования - игнорируем
                if (socketReaction.Emote.Name != "Like" &&
                    socketReaction.Emote.Name != "Dislike") return;

                // получаем сообщение из базы
                var contentMessage = await _communityDescService.GetContentMessage(
                    (long) socketMessageChannel.Id, (long) socketReaction.MessageId);
                // отключаем реакцию пользователя в базе
                await _communityDescService.DeactivateUserVote(
                    (long) socketReaction.UserId, contentMessage.Id,
                    socketReaction.Emote.Name == "Like" ? Vote.Like : Vote.Dislike);
            }
        }
    }
}
