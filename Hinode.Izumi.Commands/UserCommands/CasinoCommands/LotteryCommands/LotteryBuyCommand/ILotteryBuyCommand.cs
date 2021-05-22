using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryBuyCommand
{
    public interface ILotteryBuyCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
