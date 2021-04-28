using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Framework.Autofac;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserJoinedService
{
    [InjectableService]
    public class UserJoinedService : IUserJoinedService
    {
        public async Task Execute(SocketGuildUser guildUser)
        {
            await Task.CompletedTask;
        }
    }
}
