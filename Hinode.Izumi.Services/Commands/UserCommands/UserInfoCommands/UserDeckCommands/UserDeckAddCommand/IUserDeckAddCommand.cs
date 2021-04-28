using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckAddCommand
{
    public interface IUserDeckAddCommand
    {
        Task Execute(SocketCommandContext context, long cardId);
    }
}