using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InteractionCommands.FamilyKickUserCommand
{
    public interface IFamilyKickUserCommand
    {
        Task Execute(SocketCommandContext context, string username);
    }
}