using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldPlantCommand
{
    public interface IFieldPlantCommand
    {
        Task Execute(SocketCommandContext context, long fieldId, string seedNamePattern);
    }
}