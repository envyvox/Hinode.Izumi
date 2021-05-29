using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryGiftCommand
{
    public interface ILotteryGiftCommand
    {
        Task Execute(SocketCommandContext context, string namePattern);
    }
}
