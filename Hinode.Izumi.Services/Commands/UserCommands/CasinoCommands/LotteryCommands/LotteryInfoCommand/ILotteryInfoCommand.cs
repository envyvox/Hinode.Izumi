using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryInfoCommand
{
    public interface ILotteryInfoCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
