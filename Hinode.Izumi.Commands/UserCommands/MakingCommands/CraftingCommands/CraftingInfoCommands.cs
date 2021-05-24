using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingAlcoholInfoCommand;
using Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingAlcoholListCommand;
using Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemInfoCommand;
using Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemListCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands
{
    [Group("изготовление")]
    [CommandCategory(CommandCategory.Crafting)]
    [IzumiRequireRegistry]
    public class CraftingInfoCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ICraftingItemListCommand _craftingItemListCommand;
        private readonly ICraftingAlcoholListCommand _craftingAlcoholListCommand;
        private readonly ICraftingItemInfoCommand _craftingItemInfoCommand;
        private readonly ICraftingAlcoholInfoCommand _craftingAlcoholInfoCommand;

        public CraftingInfoCommands(ICraftingItemListCommand craftingItemListCommand,
            ICraftingAlcoholListCommand craftingAlcoholListCommand, ICraftingItemInfoCommand craftingItemInfoCommand,
            ICraftingAlcoholInfoCommand craftingAlcoholInfoCommand)
        {
            _craftingItemListCommand = craftingItemListCommand;
            _craftingAlcoholListCommand = craftingAlcoholListCommand;
            _craftingItemInfoCommand = craftingItemInfoCommand;
            _craftingAlcoholInfoCommand = craftingAlcoholInfoCommand;
        }

        [Command("предметов")]
        [Summary("Посмотреть список изготавливаемых предметов")]
        public async Task CraftingItemListTask() =>
            await _craftingItemListCommand.Execute(Context);

        [Command("алкоголя")]
        [Summary("Посмотреть список изготавливаемого алкоголя")]
        public async Task CraftingAlcoholListTask() =>
            await _craftingAlcoholListCommand.Execute(Context);

        [Command("предмета")]
        [Summary("Посмотреть информацию о изготовлении предмета с указанным номером")]
        [CommandUsage("!изготовление предмета 1", "!изготовление предмета 5")]
        public async Task CraftingItemInfoTask(
            [Summary("Номер предмета")] long itemId) =>
            await _craftingItemInfoCommand.Execute(Context, itemId);

        [Command("алкоголя")]
        [Summary("Посмотерть информацию о изготовлении алкоголя с указанным номером")]
        [CommandUsage("!изготовление алкоголя 1", "!изготовление алкоголя 3")]
        public async Task CraftingAlcoholInfoTask(
            [Summary("Номер алкоголя")] long alcoholId) =>
            await _craftingAlcoholInfoCommand.Execute(Context, alcoholId);

        [Command("предмета")]
        [Summary("Посмотреть информацию о изготовлении предмета с указанным названием")]
        [CommandUsage("!изготовление предмета палка", "!изготовление предмета ткань")]
        public async Task CraftingItemInfoTask(
            [Summary("Название предмета")] [Remainder] string itemNamePattern) =>
            await _craftingItemInfoCommand.Execute(Context, itemNamePattern);

        [Command("алкоголя")]
        [Summary("Посмотерть информацию о изготовлении алкоголя с указанным названием")]
        [CommandUsage("!изготовление алкоголя пиво", "!изготовление алкоголя вино")]
        public async Task CraftingAlcoholInfoTask(
            [Summary("Название алкоголя")] [Remainder] string alcoholNamePattern) =>
            await _craftingAlcoholInfoCommand.Execute(Context, alcoholNamePattern);
    }
}
