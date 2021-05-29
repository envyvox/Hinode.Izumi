using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyCheckInfoCommand
{
    public interface IFamilyCheckInfoCommand
    {
        Task Execute(SocketCommandContext context, string familyName);
    }
}
