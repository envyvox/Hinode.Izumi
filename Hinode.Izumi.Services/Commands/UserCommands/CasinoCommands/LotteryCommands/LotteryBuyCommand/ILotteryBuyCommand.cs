using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryBuyCommand
{
    public interface ILotteryBuyCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
