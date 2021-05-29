using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCommand
{
    public interface IUserInventoryCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
