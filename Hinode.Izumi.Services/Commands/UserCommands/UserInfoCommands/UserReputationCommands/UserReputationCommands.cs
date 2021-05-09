using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserReputationCommands.UserReputationInfoCommand;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserReputationCommands.UserReputationListCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserReputationCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserReputationCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IUserReputationListCommand _userReputationListCommand;
        private readonly IUserReputationInfoCommand _userReputationInfoCommand;

        public UserReputationCommands(IUserReputationListCommand userReputationListCommand,
            IUserReputationInfoCommand userReputationInfoCommand)
        {
            _userReputationListCommand = userReputationListCommand;
            _userReputationInfoCommand = userReputationInfoCommand;
        }

        [Command("репутация"), Alias("reputation")]
        public async Task UserReputationTask() =>
            await _userReputationListCommand.Execute(Context);

        [Command("репутация"), Alias("reputation")]
        public async Task UserReputationTask(Reputation reputation) =>
            await _userReputationInfoCommand.Execute(Context, reputation);
    }
}
