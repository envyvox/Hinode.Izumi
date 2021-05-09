using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.MessageReceived
{
    public interface IMessageReceived
    {
        Task Execute(DiscordSocketClient socketClient, SocketMessage socketMessage);
    }
}
