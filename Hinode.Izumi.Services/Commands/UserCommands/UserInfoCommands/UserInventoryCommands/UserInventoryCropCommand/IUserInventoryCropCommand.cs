using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCropCommand
{
    public interface IUserInventoryCropCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
