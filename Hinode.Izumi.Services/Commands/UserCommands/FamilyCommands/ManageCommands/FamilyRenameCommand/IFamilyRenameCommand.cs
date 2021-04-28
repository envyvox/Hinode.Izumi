using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyRenameCommand
{
    public interface IFamilyRenameCommand
    {
        Task Execute(SocketCommandContext context, string newFamilyName);
    }
}