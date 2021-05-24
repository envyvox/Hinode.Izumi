using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCommand;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCropCommand;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryFishCommand;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryFoodCommand;
using Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventorySeedCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands
{
    [CommandCategory(CommandCategory.UserInfo, CommandCategory.Inventory)]
    [Group("инвентарь"), Alias("inventory")]
    [IzumiRequireRegistry]
    public class UserInventoryCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IUserInventoryCommand _userInventoryCommand;
        private readonly IUserInventorySeedCommand _userInventorySeedCommand;
        private readonly IUserInventoryCropCommand _userInventoryCropCommand;
        private readonly IUserInventoryFishCommand _userInventoryFishCommand;
        private readonly IUserInventoryFoodCommand _userInventoryFoodCommand;

        public UserInventoryCommands(IUserInventoryCommand userInventoryCommand,
            IUserInventorySeedCommand userInventorySeedCommand, IUserInventoryCropCommand userInventoryCropCommand,
            IUserInventoryFishCommand userInventoryFishCommand, IUserInventoryFoodCommand userInventoryFoodCommand)
        {
            _userInventoryCommand = userInventoryCommand;
            _userInventorySeedCommand = userInventorySeedCommand;
            _userInventoryCropCommand = userInventoryCropCommand;
            _userInventoryFishCommand = userInventoryFishCommand;
            _userInventoryFoodCommand = userInventoryFoodCommand;
        }

        [Command("")]
        [Summary("Посмотреть свой инвентарь")]
        public async Task UserInventoryTask() =>
            await _userInventoryCommand.Execute(Context);

        [Command("семена"), Alias("seed")]
        [Summary("Посмотреть имеющиеся семена")]
        public async Task UserInventorySeedTask() =>
            await _userInventorySeedCommand.Execute(Context);

        [Command("урожай"), Alias("crop")]
        [Summary("Посмотреть имеющийся урожай")]
        public async Task UserInventoryCropTask() =>
            await _userInventoryCropCommand.Execute(Context);

        [Command("рыба"), Alias("fish")]
        [Summary("Посмотреть имеющуюся рыбу")]
        public async Task UserInventoryFishTask() =>
            await _userInventoryFishCommand.Execute(Context);

        [Command("блюда"), Alias("food")]
        [Summary("Посмотреть имеющиеся блюда")]
        public async Task UserInventoryFoodTask() =>
            await _userInventoryFoodCommand.Execute(Context);
    }
}
