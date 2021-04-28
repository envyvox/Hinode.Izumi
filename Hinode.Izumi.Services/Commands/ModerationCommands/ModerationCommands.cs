using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.ModerationCommands.UpdateGenderCommand;

namespace Hinode.Izumi.Services.Commands.ModerationCommands
{
    [Group("mod")]
    [IzumiRequireRole(DiscordRole.Moderator)]
    public class ModerationCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IUpdateGenderCommand _updateGenderCommand;

        public ModerationCommands(IUpdateGenderCommand updateGenderCommand)
        {
            _updateGenderCommand = updateGenderCommand;
        }

        [Command("update-gender")]
        public async Task UpdateGenderTask(long userId, Gender gender) =>
            await _updateGenderCommand.Execute(Context, userId, gender);
    }
}
