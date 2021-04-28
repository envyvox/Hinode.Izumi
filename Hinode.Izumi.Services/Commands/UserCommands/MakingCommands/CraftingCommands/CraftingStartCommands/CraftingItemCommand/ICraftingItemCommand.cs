using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingStartCommands.CraftingItemCommand
{
    public interface ICraftingItemCommand
    {
        Task Execute(SocketCommandContext context, long craftingId, long amount);
    }
}
