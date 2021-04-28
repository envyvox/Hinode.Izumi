using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.ListCommands
{
    public interface IShopProjectCommand
    {
        Task Execute(SocketCommandContext context);
    }
}