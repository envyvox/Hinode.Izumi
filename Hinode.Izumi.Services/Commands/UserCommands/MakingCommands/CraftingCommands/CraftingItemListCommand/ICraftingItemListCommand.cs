using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingListCommands.CraftingItemListCommand
{
    public interface ICraftingItemListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}