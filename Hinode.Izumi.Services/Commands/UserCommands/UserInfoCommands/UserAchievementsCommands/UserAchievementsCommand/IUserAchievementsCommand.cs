using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserAchievementsCommands.UserAchievementsCommand
{
    public interface IUserAchievementsCommand
    {
        Task Execute(SocketCommandContext context);
    }
}
