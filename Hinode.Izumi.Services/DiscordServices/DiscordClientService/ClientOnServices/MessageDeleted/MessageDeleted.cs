using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Commands;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageDeleted
{
    [InjectableService]
    public class MessageDeleted : IMessageDeleted
    {
        private readonly IMediator _mediator;

        public MessageDeleted(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel)
        {
            // убеждаемся что мы получили сообщение
            var msg = await message.GetOrDownloadAsync();
            // получаем из них каналы доски сообщества
            var communityDescChannels = await _mediator.Send(new GetCommunityDescChannelsQuery());

            // если сообщение находилось в канале доски сообщества
            if (communityDescChannels.Contains(channel.Id))
                // удаляем его из базы
                await _mediator.Send(new DeleteContentMessageCommand((long) channel.Id, (long) msg.Id));
        }
    }
}
