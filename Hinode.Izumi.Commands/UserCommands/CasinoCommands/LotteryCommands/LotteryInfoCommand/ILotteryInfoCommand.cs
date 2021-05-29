using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryInfoCommand
{
    public interface ILotteryInfoCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
