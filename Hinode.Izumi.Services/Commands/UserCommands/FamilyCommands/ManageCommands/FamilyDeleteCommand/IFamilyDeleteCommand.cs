using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyDeleteCommand
{
    public interface IFamilyDeleteCommand
    {
        Task Execute(SocketCommandContext context);
    }
}