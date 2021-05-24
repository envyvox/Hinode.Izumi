using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyRegisterCommand
{
    public interface IFamilyRegisterCommand
    {
        Task Execute(SocketCommandContext context, string familyName);
    }
}
