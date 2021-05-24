using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserCollectionCommands.UserCollectionCategoryCommand;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserCollectionCommands.UserCollectionCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserCollectionCommands
{
    [CommandCategory(CommandCategory.UserInfo, CommandCategory.Collection)]
    [Group("коллекция"), Alias("collection")]
    [IzumiRequireRegistry]
    public class UserCollectionCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IUserCollectionCommand _userCollectionCommand;
        private readonly IUserCollectionCategoryCommand _userCollectionCategoryCommand;

        public UserCollectionCommands(IUserCollectionCommand userCollectionCommand,
            IUserCollectionCategoryCommand userCollectionCategoryCommand)
        {
            _userCollectionCommand = userCollectionCommand;
            _userCollectionCategoryCommand = userCollectionCategoryCommand;
        }

        [Command("")]
        [Summary("Посмотреть доступные категории коллекции")]
        public async Task UserCollectionTask() =>
            await _userCollectionCommand.Execute(Context);

        [Command("")]
        [Summary("Посмотреть свою коллекцию в указаной категории")]
        [CommandUsage("!коллекция 1", "!коллекция 5")]
        public async Task UserCollectionCategoryTask(
            [Summary("Номер категории")] CollectionCategory category) =>
            await _userCollectionCategoryCommand.Execute(Context, category);
    }
}
