using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands
{
    public interface IShopBuyEventCommand
    {
        Task Execute(SocketCommandContext context, long itemId, string namePattern = null);
    }
}
