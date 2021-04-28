using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserLeftService
{
    public interface IUserLeftService
    {
        Task Execute(SocketGuildUser guildUser);
    }
}
