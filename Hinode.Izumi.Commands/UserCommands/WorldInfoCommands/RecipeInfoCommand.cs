using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.WorldInfoCommands
{
    [CommandCategory(CommandCategory.Cooking, CommandCategory.WorldInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class RecipeInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public RecipeInfoCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
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
            [Summary("Название блюда")] [Remainder]
            string foodName)
        {
            // получаем локализацию блюда
            var foodLocal = await _local.GetLocalizationByLocalizedWord(LocalizationCategory.Food, foodName);
            // выводим рецепт
            await RecipeInfoTask(foodLocal.ItemId);
        }

        private async Task RecipeInfoTask(long foodId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем блюдо
            var food = await _mediator.Send(new GetFoodQuery(foodId));
            // получаем ингредиенты
            var ingredients = await _mediator.Send(new DisplayFoodIngredientsQuery(food.Id));
            // проверяем наличие рецепта у пользователя
            var checkRecipe = await _mediator.Send(new CheckUserHasRecipeQuery((long) Context.User.Id, food.Id));
            // получаем себестоимость блюда
            var costPrice = await _mediator.Send(new GetFoodCostPriceQuery(food.Id));
            // получаем стоимость приготовления
            var cookingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice));
            // получаем количество восстанавливаемой блюдом энергии
            var energy = await _mediator.Send(new GetFoodEnergyRechargeQuery(costPrice, cookingPrice));

            var embed = new EmbedBuilder()
                // название рецепта
                .WithTitle($"`{food.Id}` {emotes.GetEmoteOrBlank("Recipe")} {_local.Localize(food.Name, 2)}")
                // изображение приготовления
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Cooking)))
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

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
