using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingAlcoholInfoCommand
{
    public interface ICraftingAlcoholInfoCommand
    {
        Task Execute(SocketCommandContext context, long alcoholId);
        Task Execute(SocketCommandContext context, string alcoholNamePattern);
    }
}
