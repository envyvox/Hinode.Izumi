using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.FamilyEnums;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.InteractionCommands.FamilyUpdateUserStatusCommand
{
    public interface IFamilyUpdateUserStatusCommand
    {
        Task Execute(SocketCommandContext context, UserInFamilyStatus newStatus, string username);
    }
}
