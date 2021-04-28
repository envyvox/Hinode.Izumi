using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands
{
    public interface IShopBuyProjectCommand
    {
        Task Execute(SocketCommandContext context, long projectId);
    }
}