using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingInfoCommands.CraftingAlcoholInfoCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingInfoCommands.CraftingItemInfoCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingListCommands.CraftingAlcoholListCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingListCommands.CraftingItemListCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingStartCommands.CraftingAlcoholCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingStartCommands.CraftingItemCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands
{
    [Group("изготовление"), Alias("изготовить", "crafting", "craft")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class CraftingCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ICraftingAlcoholListCommand _craftingAlcoholListCommand;
        private readonly ICraftingItemListCommand _craftingItemListCommand;
        private readonly ICraftingItemCommand _craftingItemCommand;
        private readonly ICraftingAlcoholCommand _craftingAlcoholCommand;
        private readonly ICraftingItemInfoCommand _craftingItemInfoCommand;
        private readonly ICraftingAlcoholInfoCommand _craftingAlcoholInfoCommand;

        public CraftingCommands(ICraftingAlcoholListCommand craftingAlcoholListCommand,
            ICraftingItemListCommand craftingItemListCommand, ICraftingItemCommand craftingItemCommand,
            ICraftingAlcoholCommand craftingAlcoholCommand, ICraftingItemInfoCommand craftingItemInfoCommand,
            ICraftingAlcoholInfoCommand craftingAlcoholInfoCommand)
        {
            _craftingAlcoholListCommand = craftingAlcoholListCommand;
            _craftingItemListCommand = craftingItemListCommand;
            _craftingItemCommand = craftingItemCommand;
            _craftingAlcoholCommand = craftingAlcoholCommand;
            _craftingItemInfoCommand = craftingItemInfoCommand;
            _craftingAlcoholInfoCommand = craftingAlcoholInfoCommand;
        }

        [Command]
        public async Task CraftingListTask(string input)
        {
            switch (input)
            {
                case "предметов" or "items":
                    await _craftingItemListCommand.Execute(Context);
                    break;
                case "алкоголя" or "alcohols":
                    await _craftingAlcoholListCommand.Execute(Context);
                    break;
            }
        }

        [Command]
        public async Task CraftingStartTask(string input, long itemId, long amount = 1)
        {
            switch (input)
            {
                case "предмета":
                    await _craftingItemInfoCommand.Execute(Context, itemId);
                    break;
                case "алкоголя":
                    await _craftingAlcoholInfoCommand.Execute(Context, itemId);
                    break;
                case "предмет":
                    await _craftingItemCommand.Execute(Context, itemId, amount);
                    break;
                case "алкоголь":
                    await _craftingAlcoholCommand.Execute(Context, itemId, amount);
                    break;
            }
        }
    }
}
