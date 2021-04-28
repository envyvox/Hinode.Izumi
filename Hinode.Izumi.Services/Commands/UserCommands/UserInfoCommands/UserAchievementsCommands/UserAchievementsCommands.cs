using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserAchievementsCommands.UserAchievementsCategoryCommand;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserAchievementsCommands.UserAchievementsCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserAchievementsCommands
{
    [Group("достижения"), Alias("achievements")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserAchievementsCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IUserAchievementsCommand _userAchievementsCommand;
        private readonly IUserAchievementsCategoryCommand _userAchievementsCategoryCommand;

        public UserAchievementsCommands(IUserAchievementsCommand userAchievementsCommand,
            IUserAchievementsCategoryCommand userAchievementsCategoryCommand)
        {
            _userAchievementsCommand = userAchievementsCommand;
            _userAchievementsCategoryCommand = userAchievementsCategoryCommand;
        }

        [Command]
        public async Task UserAchievementsCommandTask() =>
            await _userAchievementsCommand.Execute(Context);

        [Command]
        public async Task UserAchievementsCategoryTask(AchievementCategory category) =>
            await _userAchievementsCategoryCommand.Execute(Context, category);
    }
}
