using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Framework.Autofac;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.GuildMemberUpdatedService
{
    [InjectableService]
    public class GuildMemberUpdatedService : IGuildMemberUpdatedService
    {
        public async Task Execute(DiscordSocketClient socketClient, SocketGuildUser oldGuildUser,
            SocketGuildUser newGuildUser)
        {
            await Task.CompletedTask;
        }
    }
}
