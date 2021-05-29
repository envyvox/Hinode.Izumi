using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemListCommand
{
    public interface ICraftingItemListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
