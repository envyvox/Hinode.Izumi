using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserJoinedService
{
    public interface IUserJoinedService
    {
        Task Execute(SocketGuildUser guildUser);
    }
}
