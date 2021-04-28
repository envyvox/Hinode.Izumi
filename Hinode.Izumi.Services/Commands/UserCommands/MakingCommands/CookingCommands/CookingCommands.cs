using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CookingCommands.CookingListCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CookingCommands.CookingStartCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CookingCommands
{
    [Group("приготовление"), Alias("приготовить", "cooking", "cook")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class CookingCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ICookingListCommand _cookingListCommand;
        private readonly ICookingStartCommand _cookingStartCommand;

        public CookingCommands(ICookingListCommand cookingListCommand, ICookingStartCommand cookingStartCommand)
        {
            _cookingListCommand = cookingListCommand;
            _cookingStartCommand = cookingStartCommand;
        }

        [Command]
        public async Task CookingListTask(long masteryBracket = 0) =>
            await _cookingListCommand.Execute(Context, masteryBracket);

        [Command]
        public async Task CookingStartTask(long foodId, long amount) =>
            await _cookingStartCommand.Execute(Context, foodId, amount);
    }
}
