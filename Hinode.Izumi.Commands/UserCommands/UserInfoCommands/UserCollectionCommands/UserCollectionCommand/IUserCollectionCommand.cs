using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserCollectionCommands.UserCollectionCommand
{
    public interface IUserCollectionCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
