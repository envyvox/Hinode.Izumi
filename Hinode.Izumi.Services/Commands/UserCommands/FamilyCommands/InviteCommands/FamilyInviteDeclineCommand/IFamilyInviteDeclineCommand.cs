using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteDeclineCommand
{
    public interface IFamilyInviteDeclineCommand
    {
        Task Execute(SocketCommandContext context, long inviteId);
    }
}