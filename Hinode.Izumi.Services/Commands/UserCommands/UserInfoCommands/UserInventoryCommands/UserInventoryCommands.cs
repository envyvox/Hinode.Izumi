using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCommand;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCropCommand;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryFishCommand;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryFoodCommand;
using Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventorySeedCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands
{
    [Group("инвентарь"), Alias("inventory")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
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

        [Command]
        public async Task UserInventoryTask() =>
            await _userInventoryCommand.Execute(Context);

        [Command("семена"), Alias("seed")]
        public async Task UserInventorySeedTask() =>
            await _userInventorySeedCommand.Execute(Context);

        [Command("урожай"), Alias("crop")]
        public async Task UserInventoryCropTask() =>
            await _userInventoryCropCommand.Execute(Context);

        [Command("рыба"), Alias("fish")]
        public async Task UserInventoryFishTask() =>
            await _userInventoryFishCommand.Execute(Context);

        [Command("блюда"), Alias("food")]
        public async Task UserInventoryFoodTask() =>
            await _userInventoryFoodCommand.Execute(Context);
    }
}
