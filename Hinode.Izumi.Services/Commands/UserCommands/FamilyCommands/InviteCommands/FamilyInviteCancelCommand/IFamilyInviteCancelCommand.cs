using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteCancelCommand
{
    public interface IFamilyInviteCancelCommand
    {
        Task Execute(SocketCommandContext context, long inviteId);
    }
}