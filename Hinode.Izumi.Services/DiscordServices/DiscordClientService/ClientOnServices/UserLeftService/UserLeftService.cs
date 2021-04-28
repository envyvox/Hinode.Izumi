using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Framework.Autofac;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserLeftService
{
    [InjectableService]
    public class UserLeftService : IUserLeftService
    {
        public async Task Execute(SocketGuildUser guildUser)
        {
            await Task.CompletedTask;
        }
    }
}
