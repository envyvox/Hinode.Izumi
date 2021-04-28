using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldBuyCommand
{
    public interface IFieldBuyCommand
    {
        Task Execute(SocketCommandContext context);
    }
}