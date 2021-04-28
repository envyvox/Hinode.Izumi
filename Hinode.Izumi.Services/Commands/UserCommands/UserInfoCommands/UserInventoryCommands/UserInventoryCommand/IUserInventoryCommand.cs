using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCommand
{
    public interface IUserInventoryCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
