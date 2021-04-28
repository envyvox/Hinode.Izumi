using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.GuildMemberUpdatedService
{
    public interface IGuildMemberUpdatedService
    {
        Task Execute(DiscordSocketClient socketClient, SocketGuildUser oldGuildUser, SocketGuildUser newGuildUser);
    }
}
