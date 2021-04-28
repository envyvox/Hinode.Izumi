using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldCollectCommand
{
    public interface IFieldCollectCommand
    {
        Task Execute(SocketCommandContext context, long fieldId);
    }
}
