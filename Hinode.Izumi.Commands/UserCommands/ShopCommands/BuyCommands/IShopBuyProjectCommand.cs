using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands
{
    public interface IShopBuyProjectCommand
    {
        Task Execute(SocketCommandContext context, long projectId);
    }
}
