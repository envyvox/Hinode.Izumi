using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.ModerationCommands.MuteCommand;
using Hinode.Izumi.Services.Commands.ModerationCommands.UpdateGenderCommand;

namespace Hinode.Izumi.Services.Commands.ModerationCommands
{
    [Group("mod")]
    [IzumiRequireRole(DiscordRole.Moderator)]
    public class ModerationCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IUpdateGenderCommand _updateGenderCommand;
        private readonly IMuteCommand _muteCommand;

        public ModerationCommands(IUpdateGenderCommand updateGenderCommand, IMuteCommand muteCommand)
        {
            _updateGenderCommand = updateGenderCommand;
            _muteCommand = muteCommand;
        }

        [Command("update-gender")]
        public async Task UpdateGenderTask(long userId, Gender gender) =>
            await _updateGenderCommand.Execute(Context, userId, gender);

        [Command("mute")]
        public async Task MuteTask(long userId, long duration = 5, [Remainder] string reason = null) =>
            await _muteCommand.Execute(Context, userId, duration, reason);

        [Command("mute")]
        public async Task MuteTask(IUser user, long duration = 5, [Remainder] string reason = null) =>
            await _muteCommand.Execute(Context, (long) user.Id, duration, reason);
    }
}
