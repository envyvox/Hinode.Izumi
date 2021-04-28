using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventorySeedCommand
{
    public interface IUserInventorySeedCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
