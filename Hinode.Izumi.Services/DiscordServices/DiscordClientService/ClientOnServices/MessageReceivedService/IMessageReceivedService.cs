using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageReceivedService
{
    public interface IMessageReceivedService
    {
        Task Execute(DiscordSocketClient socketClient, SocketMessage socketMessage);
    }
}
