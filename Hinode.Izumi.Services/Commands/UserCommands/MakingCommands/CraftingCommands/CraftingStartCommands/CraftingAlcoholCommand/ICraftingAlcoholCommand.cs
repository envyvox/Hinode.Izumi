using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingStartCommands.CraftingAlcoholCommand
{
    public interface ICraftingAlcoholCommand
    {
        Task Execute(SocketCommandContext context, long alcoholId, long amount);
    }
}
