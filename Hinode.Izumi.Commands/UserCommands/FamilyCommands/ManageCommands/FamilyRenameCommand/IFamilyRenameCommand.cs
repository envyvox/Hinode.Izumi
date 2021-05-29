using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyRenameCommand
{
    public interface IFamilyRenameCommand
    {
        Task Execute(SocketCommandContext context, string newFamilyName);
    }
}
