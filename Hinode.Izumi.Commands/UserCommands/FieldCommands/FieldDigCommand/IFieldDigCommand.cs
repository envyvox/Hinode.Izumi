using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldDigCommand
{
    public interface IFieldDigCommand
    {
        Task Execute(SocketCommandContext context, long fieldId);
    }
}
