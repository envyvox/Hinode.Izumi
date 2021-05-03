using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.EmoteService.Models;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.FoodService.Models;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopRecipeCommand : IShopRecipeCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;
        private readonly IFoodService _foodService;
        private readonly ICalculationService _calc;
        private readonly IIngredientService _ingredientService;
        private readonly IPropertyService _propertyService;

        private Dictionary<string, EmoteModel> _emotes;

        public ShopRecipeCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IImageService imageService, IFoodService foodService, ICalculationService calc,
            IIngredientService ingredientService, IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _imageService = imageService;
            _foodService = foodService;
            _calc = calc;
            _ingredientService = ingredientService;
            _propertyService = propertyService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            _emotes = await _emoteService.GetEmotes();
            // получаем текущий сезон
            var season = (Season) await _propertyService.GetPropertyValue(Property.CurrentSeason);
            // получаем еду из базы
            var foods = await _foodService.GetAllRecipeSellableFood();

            var embed = new EmbedBuilder()
                // рассказываем как покупать рецепты
                .WithDescription(
                    IzumiReplyMessage.ShopRecipeDesc.Parse(
                        _emotes.GetEmoteOrBlank("Recipe")) +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина рецептов
                .WithImageUrl(await _imageService.GetImageUrl(Image.ShopRecipe))
                // разбиваем все рецепты по категориям мастерства
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery0.Parse(),
                    await Display(foods.Where(x => x.Mastery == 0), (long) context.User.Id, season))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery50.Parse(),
                    await Display(foods.Where(x => x.Mastery == 50), (long) context.User.Id, season))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery100.Parse(),
                    await Display(foods.Where(x => x.Mastery == 100), (long) context.User.Id, season))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery150.Parse(),
                    await Display(foods.Where(x => x.Mastery == 150), (long) context.User.Id, season))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery200.Parse(),
                    await Display(foods.Where(x => x.Mastery == 200), (long) context.User.Id, season))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery250.Parse(),
                    await Display(foods.Where(x => x.Mastery == 250), (long) context.User.Id, season));

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Заполняет строку с локализированной информацией о рецептах (иконка, стоимость, название).
        /// </summary>
        /// <param name="foods">Массив блюд.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="season">Текущий сезон.</param>
        /// <returns>Локализированная строка с информацией о рецептах.</returns>
        private async Task<string> Display(IEnumerable<FoodModel> foods, long userId, Season season)
        {
            var displayString = string.Empty;

            // добавляем информацию о каждом рецепте
            foreach (var food in foods)
            {
                // получаем сезоны блюда
                var foodSeasons = await _ingredientService.GetFoodSeasons(food.Id);
                // если блюдо не содержит текущий сезон - пропускаем
                if (!foodSeasons.Contains(season)) continue;

                // проверяем есть ли этот рецепт у пользователя
                var hasRecipe = await _foodService.CheckUserRecipe(userId, food.Id);
                // если у пользователя уже есть этот рецепт - пропускаем
                if (hasRecipe) continue;

                // определяем стоимость рецепта
                var recipeCost = await _calc.FoodRecipePrice(
                    // получаем себестоимость блюда
                    await _ingredientService.GetFoodCostPrice(food.Id));

                var info = IzumiReplyMessage.ShopRecipeFieldDesc.Parse(
                    food.Id, _emotes.GetEmoteOrBlank("Recipe"), _local.Localize(food.Name, 2),
                    _emotes.GetEmoteOrBlank(Currency.Ien.ToString()), recipeCost,
                    _local.Localize(Currency.Ien.ToString(), recipeCost));

                // убеждаемся что добавленный рецепт не превысит максимальное количество символов
                if (displayString.Length + info.Length > 1024) continue;

                displayString += info;
            }

            return displayString.Length > 0
                ? displayString
                // если категория с рецептами пуская, выводим сообщение о том, что пользователь все купил
                : IzumiReplyMessage.ShopRecipeCategoryListEmpty.Parse(_emotes.GetEmoteOrBlank("Recipe"));
        }
    }
}
