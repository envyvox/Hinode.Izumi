using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.ExploreCommands.ExploreGardenCommand
{
    public interface IExploreGardenCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
