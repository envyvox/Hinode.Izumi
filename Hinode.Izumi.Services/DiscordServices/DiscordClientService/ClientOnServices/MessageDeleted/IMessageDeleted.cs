using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageDeleted
{
    public interface IMessageDeleted
    {
        Task Execute(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel);
    }
}
