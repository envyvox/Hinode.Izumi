using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryFishCommand
{
    public interface IUserInventoryFishCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
