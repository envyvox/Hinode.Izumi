using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingInfoCommands.
    CraftingAlcoholInfoCommand
{
    public interface ICraftingAlcoholInfoCommand
    {
        Task Execute(SocketCommandContext context, long alcoholId);
    }
}
