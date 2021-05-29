using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.ModerationCommands.MuteCommand
{
    public interface IMuteCommand
    {
        Task Execute(SocketCommandContext context, long userId, long duration, string reason = null);
    }
}
