using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands
{
    public interface IShopProductCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
