using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Framework.Autofac;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserJoined
{
    [InjectableService]
    public class UserJoined : IUserJoined
    {
        public async Task Execute(SocketGuildUser guildUser)
        {
            await Task.CompletedTask;
        }
    }
}
