using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands
{
    public interface IShopBuySeedCommand
    {
        Task Execute(SocketCommandContext context, long seedId, long amount);
    }
}
