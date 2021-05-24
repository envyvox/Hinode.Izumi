using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands
{
    public interface IShopSeedCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
