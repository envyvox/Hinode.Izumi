using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands
{
    public interface IShopBuyCertificateCommand
    {
        Task Execute(SocketCommandContext context, long certificateId);
    }
}
