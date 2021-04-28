using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldInfoCommand
{
    public interface IFieldInfoCommand
    {
        Task Execute(SocketCommandContext context);
    }
}