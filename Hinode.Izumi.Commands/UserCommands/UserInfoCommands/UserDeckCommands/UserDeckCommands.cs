using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckAddCommand;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckListCommand;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserDeckCommands.UserDeckRemoveCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserDeckCommands
{
    [CommandCategory(CommandCategory.Cards, CommandCategory.UserInfo)]
    [Group("колода"), Alias("deck")]
    [IzumiRequireRegistry]
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

        [Command("")]
        [Summary("Посмотреть свою текущую колоду")]
        public async Task UserDeckListTask() =>
            await _userDeckListCommand.Execute(Context);

        [Command("добавить"), Alias("add")]
        [Summary("Добавить указанную карточку в свою колоду")]
        [CommandUsage("!колода добавить 1", "!колода добавить 5")]
        public async Task UserDeckAddTask(
            [Summary("Номер карточки")] long cardId) =>
            await _userDeckAddCommand.Execute(Context, cardId);

        [Command("убрать"), Alias("remove")]
        [Summary("Убрать указанную карточку из своей колоды")]
        [CommandUsage("!колода убрать 1", "!колода убрать 5")]
        public async Task UserDeckRemove(
            [Summary("Номер карточки")] long cardId) =>
            await _userDeckRemoveCommand.Execute(Context, cardId);
    }
}
