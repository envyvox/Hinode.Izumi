using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CookingCommands.CookingListCommand
{
    public interface ICookingListCommand
    {
        Task Execute(SocketCommandContext context, long masteryBracket);
    }
}
