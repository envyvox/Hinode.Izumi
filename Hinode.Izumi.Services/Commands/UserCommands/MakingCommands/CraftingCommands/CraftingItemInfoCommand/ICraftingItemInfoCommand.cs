using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemInfoCommand
{
    public interface ICraftingItemInfoCommand
    {
        Task Execute(SocketCommandContext context, long craftingId);
        Task Execute(SocketCommandContext context, string itemNamePattern);
    }
}
