using System.Threading.Tasks;
using Discord.WebSocket;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.GuildMemberUpdated
{
    public interface IGuildMemberUpdated
    {
        Task Execute(DiscordSocketClient socketClient, SocketGuildUser oldGuildUser, SocketGuildUser newGuildUser);
    }
}
