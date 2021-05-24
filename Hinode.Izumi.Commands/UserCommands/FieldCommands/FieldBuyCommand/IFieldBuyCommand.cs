using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldBuyCommand
{
    public interface IFieldBuyCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
