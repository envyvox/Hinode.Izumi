using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CookingCommands.CookingStartCommand
{
    public interface ICookingStartCommand
    {
        Task Execute(SocketCommandContext context, long foodId, long amount);
    }
}