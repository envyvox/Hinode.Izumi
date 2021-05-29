using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteSendCommand
{
    public interface IFamilyInviteSendCommand
    {
        Task Execute(SocketCommandContext context, string username);
    }
}
