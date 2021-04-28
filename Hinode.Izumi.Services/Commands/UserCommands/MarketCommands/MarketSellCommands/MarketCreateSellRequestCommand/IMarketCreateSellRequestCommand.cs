using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketSellCommands.MarketCreateSellRequestCommand
{
    public interface IMarketCreateSellRequestCommand
    {
        Task Execute(SocketCommandContext context, MarketCategory category, string pattern, long price,
            long amount = 1);
    }
}
