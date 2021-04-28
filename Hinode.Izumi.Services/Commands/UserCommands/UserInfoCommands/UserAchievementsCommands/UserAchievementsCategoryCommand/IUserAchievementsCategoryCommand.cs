using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.AchievementEnums;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserAchievementsCommands.UserAchievementsCategoryCommand
{
    public interface IUserAchievementsCategoryCommand
    {
        Task Execute(SocketCommandContext context, AchievementCategory category);
    }
}
