using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingListCommands.CraftingAlcoholListCommand
{
    public interface ICraftingAlcoholListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}