using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyInfoCommand
{
    public interface IFamilyInfoCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
