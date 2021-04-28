using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckListCommand
{
    public interface IUserDeckListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}