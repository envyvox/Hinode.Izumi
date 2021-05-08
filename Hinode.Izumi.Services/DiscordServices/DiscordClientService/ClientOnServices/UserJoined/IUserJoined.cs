using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserJoined
{
    public interface IUserJoined
    {
        Task Execute(SocketGuildUser guildUser);
    }
}
