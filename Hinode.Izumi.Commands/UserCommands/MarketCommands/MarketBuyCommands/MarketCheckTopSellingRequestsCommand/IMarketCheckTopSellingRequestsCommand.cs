using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketBuyCommands.
    MarketCheckTopSellingRequestsCommand
{
    public interface IMarketCheckTopSellingRequestsCommand
    {
        Task Execute(SocketCommandContext context, MarketCategory category, string pattern = null);
    }
}
