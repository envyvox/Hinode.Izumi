using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteListCommand
{
    public interface IFamilyInviteListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}