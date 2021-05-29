using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldCollectCommand
{
    public interface IFieldCollectCommand
    {
        Task Execute(SocketCommandContext context, long fieldId);
    }
}
