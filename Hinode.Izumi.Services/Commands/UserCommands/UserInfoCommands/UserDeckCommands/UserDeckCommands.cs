using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckAddCommand;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckListCommand;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckRemoveCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserDeckCommands
{
    [Group("колода"), Alias("deck")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserDeckCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IUserDeckListCommand _userDeckListCommand;
        private readonly IUserDeckAddCommand _userDeckAddCommand;
        private readonly IUserDeckRemoveCommand _userDeckRemoveCommand;

        public UserDeckCommands(IUserDeckListCommand userDeckListCommand, IUserDeckAddCommand userDeckAddCommand,
            IUserDeckRemoveCommand userDeckRemoveCommand)
        {
            _userDeckListCommand = userDeckListCommand;
            _userDeckAddCommand = userDeckAddCommand;
            _userDeckRemoveCommand = userDeckRemoveCommand;
        }

        [Command]
        public async Task UserDeckListTask() =>
            await _userDeckListCommand.Execute(Context);

        [Command("добавить"), Alias("add")]
        public async Task UserDeckAddTask(long cardId) =>
            await _userDeckAddCommand.Execute(Context, cardId);

        [Command("убрать"), Alias("remove")]
        public async Task UserDeckRemove(long cardId) =>
            await _userDeckRemoveCommand.Execute(Context, cardId);
    }
}
