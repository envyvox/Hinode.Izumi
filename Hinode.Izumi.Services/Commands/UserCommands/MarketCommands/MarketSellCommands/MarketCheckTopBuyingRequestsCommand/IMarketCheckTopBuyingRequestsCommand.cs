using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketSellCommands.
    MarketCheckTopBuyingRequestsCommand
{
    public interface IMarketCheckTopBuyingRequestsCommand
    {
        Task Execute(SocketCommandContext context, MarketCategory category, string pattern = null);
    }
}
