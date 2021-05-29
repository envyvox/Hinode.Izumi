using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteListCommand
{
    public interface IFamilyInviteListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
