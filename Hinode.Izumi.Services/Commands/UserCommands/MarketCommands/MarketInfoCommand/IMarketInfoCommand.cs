using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketInfoCommand
{
    public interface IMarketInfoCommand
    {
        Task Execute(SocketCommandContext context);
    }
}