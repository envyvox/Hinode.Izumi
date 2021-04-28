using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.ExploreCommands.ExploreCastleCommand
{
    public interface IExploreCastleCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
