using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CookingCommands.CookingStartCommand
{
    public interface ICookingStartCommand
    {
        Task Execute(SocketCommandContext context, long amount, long foodId);
    }
}
