using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserCollectionCommands.UserCollectionCategoryCommand;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserCollectionCommands.UserCollectionCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserCollectionCommands
{
    [Group("коллекция"), Alias("collection")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
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

        [Command]
        public async Task UserCollectionTask() =>
            await _userCollectionCommand.Execute(Context);

        [Command]
        public async Task UserCollectionCategoryTask(CollectionCategory category) =>
            await _userCollectionCategoryCommand.Execute(Context, category);
    }
}
