using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Framework.Autofac;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.GuildMemberUpdated
{
    [InjectableService]
    public class GuildMemberUpdated : IGuildMemberUpdated
    {
        public async Task Execute(DiscordSocketClient socketClient, SocketGuildUser oldGuildUser,
            SocketGuildUser newGuildUser)
        {
            await Task.CompletedTask;
        }
    }
}
