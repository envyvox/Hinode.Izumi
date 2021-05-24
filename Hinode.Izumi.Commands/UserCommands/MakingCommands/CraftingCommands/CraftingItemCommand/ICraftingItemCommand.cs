using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemCommand
{
    public interface ICraftingItemCommand
    {
        Task Execute(SocketCommandContext context, long amount, long craftingId);
        Task Execute(SocketCommandContext context, long amount, string itemNamePattern);
    }
}
