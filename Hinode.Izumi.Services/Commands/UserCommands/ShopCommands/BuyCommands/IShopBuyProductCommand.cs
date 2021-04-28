using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands
{
    public interface IShopBuyProductCommand
    {
        Task Execute(SocketCommandContext context, long productId, long amount);
    }
}
