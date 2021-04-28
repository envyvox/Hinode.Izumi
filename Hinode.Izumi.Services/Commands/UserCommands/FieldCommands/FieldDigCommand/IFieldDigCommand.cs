using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldDigCommand
{
    public interface IFieldDigCommand
    {
        Task Execute(SocketCommandContext context, long fieldId);
    }
}