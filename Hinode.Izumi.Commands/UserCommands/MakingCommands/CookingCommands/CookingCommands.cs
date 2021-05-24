using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.MakingCommands.CookingCommands.CookingListCommand;
using Hinode.Izumi.Commands.UserCommands.MakingCommands.CookingCommands.CookingStartCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.GameServices.LocalizationService;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CookingCommands
{
    [CommandCategory(CommandCategory.Cooking)]
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
        [Summary("Посмотреть доступные категории рецептов или список приобренных рецептов в указанной категории")]
        [CommandUsage("!приготовление", "!приготовление 1")]
        public async Task CookingListTask(
            [Summary("Номер категории")] long masteryBracket = 0) =>
            await _cookingListCommand.Execute(Context, masteryBracket);

        [Command("приготовить")]
        [Summary("Приготовить блюда по указанному номеру")]
        [CommandUsage("!приготовить 1 4")]
        public async Task CookingStartTask(
            [Summary("Количество")] long amount,
            [Summary("Номер блюда")] long foodId) =>
            // пробуем приготовить
            await _cookingStartCommand.Execute(Context, amount, foodId);

        [Command("приготовить")]
        [Summary("Приготовить блюда по указанному названию")]
        [CommandUsage("!приготовить 1 яичница")]
        public async Task CookingStartTask(
            [Summary("Количество")] long amount,
            [Summary("Название блюда")] [Remainder] string foodName)
        {
            // получаем локализацию блюда
            var foodLocal = await _local.GetLocalizationByLocalizedWord(LocalizationCategory.Food, foodName);
            // пробуем приготовить
            await _cookingStartCommand.Execute(Context, amount, foodLocal.ItemId);
        }
    }
}
