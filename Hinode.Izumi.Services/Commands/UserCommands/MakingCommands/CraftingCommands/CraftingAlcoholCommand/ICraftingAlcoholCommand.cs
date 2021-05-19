using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingAlcoholCommand
{
    public interface ICraftingAlcoholCommand
    {
        Task Execute(SocketCommandContext context, long amount, long alcoholId);
        Task Execute(SocketCommandContext context, long amount, string alcoholNamePattern);
    }
}
