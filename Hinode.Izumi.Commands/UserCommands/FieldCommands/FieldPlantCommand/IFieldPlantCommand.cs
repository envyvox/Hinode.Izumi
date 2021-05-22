using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldPlantCommand
{
    public interface IFieldPlantCommand
    {
        Task Execute(SocketCommandContext context, long fieldId, string seedNamePattern);
    }
}
