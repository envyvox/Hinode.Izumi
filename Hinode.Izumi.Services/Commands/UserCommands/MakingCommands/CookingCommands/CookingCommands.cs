using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CookingCommands.CookingListCommand;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CookingCommands.CookingStartCommand;
using Hinode.Izumi.Services.RpgServices.LocalizationService;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CookingCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class CookingCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ICookingListCommand _cookingListCommand;
        private readonly ICookingStartCommand _cookingStartCommand;
        private readonly ILocalizationService _local;

        public CookingCommands(ICookingListCommand cookingListCommand, ICookingStartCommand cookingStartCommand,
            ILocalizationService local)
        {
            _cookingListCommand = cookingListCommand;
            _cookingStartCommand = cookingStartCommand;
            _local = local;
        }

        [Command("приготовление")]
        public async Task CookingListTask(long masteryBracket = 0) =>
            await _cookingListCommand.Execute(Context, masteryBracket);

        [Command("приготовить")]
        public async Task CookingStartTask(long amount, long foodId) =>
            // пробуем приготовить
            await _cookingStartCommand.Execute(Context, amount, foodId);

        [Command("приготовить")]
        public async Task CookingStartTask(long amount, [Remainder] string foodName)
        {
            // получаем локализацию блюда
            var foodLocal = await _local.GetLocalizationByLocalizedWord(LocalizationCategory.Food, foodName);
            // пробуем приготовить
            await _cookingStartCommand.Execute(Context, amount, foodLocal.ItemId);
        }
    }
}
