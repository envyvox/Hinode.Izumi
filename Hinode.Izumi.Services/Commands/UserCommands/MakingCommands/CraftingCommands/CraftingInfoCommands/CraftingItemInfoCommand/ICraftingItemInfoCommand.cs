using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingInfoCommands.
    CraftingItemInfoCommand
{
    public interface ICraftingItemInfoCommand
    {
        Task Execute(SocketCommandContext context, long craftingId);
    }
}
