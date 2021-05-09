using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Framework.Autofac;

namespace Hinode.Izumi.Services.DiscordServices.DiscordClientService.ClientOnServices.UserLeft
{
    [InjectableService]
    public class UserLeft : IUserLeft
    {
        public async Task Execute(SocketGuildUser guildUser)
        {
            await Task.CompletedTask;
        }
    }
}
