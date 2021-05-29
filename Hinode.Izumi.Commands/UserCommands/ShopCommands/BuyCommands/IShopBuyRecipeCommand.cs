using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands
{
    public interface IShopBuyRecipeCommand
    {
        Task Execute(SocketCommandContext context, long foodId);
    }
}
