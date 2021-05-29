using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands
{
    public interface IShopBuyProductCommand
    {
        Task Execute(SocketCommandContext context, long productId, long amount);
    }
}
