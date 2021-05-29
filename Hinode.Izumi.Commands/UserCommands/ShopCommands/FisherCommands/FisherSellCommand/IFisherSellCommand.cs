using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.FisherCommands.FisherSellCommand
{
    public interface IFisherSellCommand
    {
        Task SellFishWithIdAndAmount(SocketCommandContext context, long fishId, long amount);
        Task SellAllFishWithId(SocketCommandContext context, long fishId, string input);
        Task SellAllFish(SocketCommandContext context, string input);
    }
}
