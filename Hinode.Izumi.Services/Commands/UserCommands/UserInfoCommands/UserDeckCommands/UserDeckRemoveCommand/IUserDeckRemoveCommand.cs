using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckRemoveCommand
{
    public interface IUserDeckRemoveCommand
    {
        Task Execute(SocketCommandContext context, long cardId);
    }
}