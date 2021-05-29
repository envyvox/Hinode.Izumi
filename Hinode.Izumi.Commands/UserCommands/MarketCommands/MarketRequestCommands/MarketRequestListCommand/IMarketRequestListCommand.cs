using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketRequestCommands.MarketRequestListCommand
{
    public interface IMarketRequestListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
