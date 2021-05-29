using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryFoodCommand
{
    public interface IUserInventoryFoodCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
