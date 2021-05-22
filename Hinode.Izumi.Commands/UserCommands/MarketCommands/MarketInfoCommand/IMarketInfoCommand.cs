using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.MarketCommands.MarketInfoCommand
{
    public interface IMarketInfoCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
