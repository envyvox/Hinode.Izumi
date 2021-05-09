using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserLeft
{
    public interface IUserLeft
    {
        Task Execute(SocketGuildUser guildUser);
    }
}
