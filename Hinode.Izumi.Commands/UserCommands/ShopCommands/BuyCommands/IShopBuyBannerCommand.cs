using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands
{
    public interface IShopBuyBannerCommand
    {
        Task Execute(SocketCommandContext context, long bannerId);
    }
}
