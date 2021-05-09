using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserReputationCommands.UserReputationListCommand
{
    public interface IUserReputationListCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
