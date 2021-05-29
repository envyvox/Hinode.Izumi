using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands
{
    public interface IShopBannerCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
