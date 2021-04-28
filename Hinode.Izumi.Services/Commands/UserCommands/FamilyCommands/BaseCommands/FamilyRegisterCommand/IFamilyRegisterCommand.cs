using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyRegisterCommand
{
    public interface IFamilyRegisterCommand
    {
        Task Execute(SocketCommandContext context, string familyName);
    }
}