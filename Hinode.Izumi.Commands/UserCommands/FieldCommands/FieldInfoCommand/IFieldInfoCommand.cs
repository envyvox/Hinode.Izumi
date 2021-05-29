using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldInfoCommand
{
    public interface IFieldInfoCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
