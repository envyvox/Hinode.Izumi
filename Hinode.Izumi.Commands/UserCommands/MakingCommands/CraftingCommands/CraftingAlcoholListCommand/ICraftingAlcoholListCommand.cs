using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingAlcoholListCommand
{
    public interface ICraftingAlcoholListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
