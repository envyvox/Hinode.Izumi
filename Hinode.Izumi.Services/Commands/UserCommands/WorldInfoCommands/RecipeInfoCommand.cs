using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.WorldInfoCommands
{
    [CommandCategory(CommandCategory.Cooking, CommandCategory.WorldInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class RecipeInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFoodService _foodService;
        private readonly IIngredientService _ingredientService;
        private readonly ICalculationService _calc;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;

        public RecipeInfoCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFoodService foodService, IIngredientService ingredientService, ICalculationService calc,
            ILocalizationService local, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _foodService = foodService;
            _ingredientService = ingredientService;
            _calc = calc;
            _local = local;
            _imageService = imageService;
        }

        [Command("рецепт"), Alias("recipe")]
        [Summary("Посмотреть рецепт блюда с указанным номером")]
        [CommandUsage("!рецепт 1", "!рецепт 4")]
        public async Task RecipeInfoCommandTask(
            [Summary("Номер блюда")] long foodId) =>
            // выводим рецепт
            await RecipeInfoTask(foodId);

        [Command("рецепт"), Alias("recipe")]
        [Summary("Посмотреть рецепт блюда с указанным названием")]
        [CommandUsage("!рецепт тортильи", "!рецепт яичницы")]
        public async Task RecipeInfoCommandTask(
            [Summary("Название блюда")] [Remainder] string foodName)
        {
            // получаем локализацию блюда
            var foodLocal = await _local.GetLocalizationByLocalizedWord(LocalizationCategory.Food, foodName);
            // выводим рецепт
            await RecipeInfoTask(foodLocal.ItemId);
        }

        private async Task RecipeInfoTask(long foodId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем блюдо
            var food = await _foodService.GetFood(foodId);
            // получаем ингредиенты
            var ingredients = await _ingredientService.DisplayFoodIngredients(food.Id);
            // проверяем наличие рецепта у пользователя
            var checkRecipe = await _foodService.CheckUserRecipe((long) Context.User.Id, food.Id);
            // получаем себестоимость блюда
            var costPrice = await _ingredientService.GetFoodCostPrice(food.Id);
            // получаем стоимость приготовления
            var cookingPrice = await _calc.CraftingPrice(costPrice);
            // получаем количество восстанавливаемой блюдом энергии
            var energy = await _calc.FoodEnergyRecharge(costPrice, cookingPrice);

            var embed = new EmbedBuilder()
                // название рецепта
                .WithTitle($"`{food.Id}` {emotes.GetEmoteOrBlank("Recipe")} {_local.Localize(food.Name, 2)}")
                // изображение приготовления
                .WithImageUrl(await _imageService.GetImageUrl(Image.Cooking))
                // рассказываем как приготовить блюдо
                .WithDescription(
                    IzumiReplyMessage.RecipeInfoDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // получаемое блюдо
                .AddField(IzumiReplyMessage.RecipeInfoFoodFieldName.Parse(),
                    IzumiReplyMessage.CookingListFieldDesc.Parse(
                        emotes.GetEmoteOrBlank(food.Name), _local.Localize(food.Name),
                        emotes.GetEmoteOrBlank("Energy"), energy, _local.Localize("Energy", energy)))
                // требуемое мастерство
                .AddField(IzumiReplyMessage.RecipeInfoMasteryFieldName.Parse(),
                    IzumiReplyMessage.RecipeInfoMasteryFieldDesc.Parse(
                        emotes.GetEmoteOrBlank(Mastery.Cooking.ToString()), food.Mastery, Mastery.Cooking.Localize()),
                    true)
                // наличие рецепта
                .AddField(IzumiReplyMessage.RecipeInfoCheckRecipeFieldName.Parse(),
                    checkRecipe
                        ? IzumiReplyMessage.RecipeInfoCheckRecipeTrue.Parse(
                            emotes.GetEmoteOrBlank("Checkmark"), emotes.GetEmoteOrBlank("Recipe"))
                        : IzumiReplyMessage.RecipeInfoCheckRecipeFalse.Parse(
                            emotes.GetEmoteOrBlank("Crossmark"), emotes.GetEmoteOrBlank("Recipe")), true)
                // необходимые ингредиенты
                .AddField(IzumiReplyMessage.RecipeInfoIngredientsFieldName.Parse(), ingredients)
                // стоимость приготовления
                .AddField(IzumiReplyMessage.RecipeInfoCookingPriceFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {cookingPrice} {_local.Localize(Currency.Ien.ToString(), cookingPrice)}",
                    true)
                // длительность приготовления
                .AddField(IzumiReplyMessage.RecipeInfoCookingTimeFieldName.Parse(),
                    food.Time.Seconds().Humanize(2, new CultureInfo("ru-RU")), true)
                // уточняем что длительность указана по-умолчанию
                .WithFooter(IzumiReplyMessage.RecipeInfoFooter.Parse());

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
