using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.ExploreCommands.ExploreGardenCommand
{
    public interface IExploreGardenCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
