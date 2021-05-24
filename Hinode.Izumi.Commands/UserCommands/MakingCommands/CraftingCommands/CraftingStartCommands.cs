using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingAlcoholCommand;
using Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands
{
    [Group("изготовить")]
    [CommandCategory(CommandCategory.Crafting)]
    [IzumiRequireRegistry]
    public class CraftingStartCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ICraftingItemCommand _craftingItemCommand;
        private readonly ICraftingAlcoholCommand _craftingAlcoholCommand;

        public CraftingStartCommands(ICraftingItemCommand craftingItemCommand,
            ICraftingAlcoholCommand craftingAlcoholCommand)
        {
            _craftingItemCommand = craftingItemCommand;
            _craftingAlcoholCommand = craftingAlcoholCommand;
        }

        [Command("предмет")]
        [Summary("Изготовить предметы с указанным номером")]
        [CommandUsage("!изготовить предмет 10 1", "!изготовить предмет 10 5")]
        public async Task CraftingItemStartTask(
            [Summary("Количество")] long amount,
            [Summary("Номер предмета")] long itemId) =>
            await _craftingItemCommand.Execute(Context, amount, itemId);

        [Command("алкоголь")]
        [Summary("Изготовить алкоголь с указанным номером")]
        [CommandUsage("!изготовить алкоголь 10 1", "!изготовить алкоголь 10 3")]
        public async Task CraftingAlcoholStartTask(
            [Summary("Количество")] long amount,
            [Summary("Номер алкоголя")] long alcoholId) =>
            await _craftingAlcoholCommand.Execute(Context, amount, alcoholId);

        [Command("предмет")]
        [Summary("Изготовить предметы с указанным названием")]
        [CommandUsage("!изготовить предмет 10 палок", "!изготовить предмет 10 тканей")]
        public async Task CraftingItemStartTask(
            [Summary("Количество")] long amount,
            [Summary("Название предмета")] [Remainder] string itemNamePattern) =>
            await _craftingItemCommand.Execute(Context, amount, itemNamePattern);

        [Command("алкоголь")]
        [Summary("Изготовить алкоголь с указанным названием")]
        [CommandUsage("!изготовить алкоголь 10 пива", "!изготовить алкоголь 10 вин")]
        public async Task CraftingAlcoholStartTask(
            [Summary("Количество")] long amount,
            [Summary("Название алкоголя")] [Remainder] string alcoholNamePattern) =>
            await _craftingAlcoholCommand.Execute(Context, amount, alcoholNamePattern);
    }
}
