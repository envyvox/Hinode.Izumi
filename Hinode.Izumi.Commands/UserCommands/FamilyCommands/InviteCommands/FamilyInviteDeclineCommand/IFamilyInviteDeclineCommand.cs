using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteDeclineCommand
{
    public interface IFamilyInviteDeclineCommand
    {
        Task Execute(SocketCommandContext context, long inviteId);
    }
}
