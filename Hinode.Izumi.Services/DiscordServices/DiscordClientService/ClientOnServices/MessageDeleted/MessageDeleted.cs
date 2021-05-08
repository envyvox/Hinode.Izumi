using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageDeleted
{
    [InjectableService]
    public class MessageDeleted : IMessageDeleted
    {
        private readonly ICommunityDescService _communityDescService;
        private readonly IDiscordGuildService _discordGuildService;

        public MessageDeleted(ICommunityDescService communityDescService,
            IDiscordGuildService discordGuildService)
        {
            _communityDescService = communityDescService;
            _discordGuildService = discordGuildService;
        }

        public async Task Execute(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel)
        {
            // убеждаемся что мы получили сообщение
            var msg = await message.GetOrDownloadAsync();
            // получаем каналы сервера
            var channels = await _discordGuildService.GetChannels();
            // получаем из них каналы доски сообщества
            var communityDescChannels = _communityDescService.CommunityDescChannels(channels);

            // если сообщение находилось в канале доски сообщества
            if (communityDescChannels.Contains(channel.Id))
                // удаляем его из базы
                await _communityDescService.RemoveContentMessage((long) channel.Id, (long) msg.Id);
        }
    }
}
