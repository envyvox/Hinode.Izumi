using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
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

        private Dictionary<string, EmoteModel> _emotes;

        public ShopRecipeCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IImageService imageService, IFoodService foodService, ICalculationService calc,
            IIngredientService ingredientService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _imageService = imageService;
            _foodService = foodService;
            _calc = calc;
            _ingredientService = ingredientService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            _emotes = await _emoteService.GetEmotes();
            // получаем еду из базы
            var foods = await _foodService.GetAllFood();

            var embed = new EmbedBuilder()
                // рассказываем как покупать рецепты
                .WithDescription(
                    IzumiReplyMessage.ShopRecipeDesc.Parse() +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина рецептов
                .WithImageUrl(await _imageService.GetImageUrl(Image.ShopRecipe))
                // разбиваем все рецепты по категориям мастерства
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery0.Parse(),
                    await Display(foods.Where(x => x.Mastery == 0)))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery50.Parse(),
                    await Display(foods.Where(x => x.Mastery == 50)))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery100.Parse(),
                    await Display(foods.Where(x => x.Mastery == 100)))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery150.Parse(),
                    await Display(foods.Where(x => x.Mastery == 150)))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery200.Parse(),
                    await Display(foods.Where(x => x.Mastery == 200)))
                .AddField(IzumiReplyMessage.ShopRecipeFieldNameMastery250.Parse(),
                    await Display(foods.Where(x => x.Mastery == 250)));

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Заполняет строку с локализированной информацией о рецептах (иконка, стоимость, название).
        /// </summary>
        /// <param name="foods">Массив еды.</param>
        /// <returns>Локализированная строка с информацией о рецептах.</returns>
        private async Task<string> Display(IEnumerable<FoodModel> foods)
        {
            var displayString = string.Empty;

            // добавляем информацию о каждом блюде
            foreach (var food in foods)
            {
                // определяем стоимость рецепта
                var recipeCost = await _calc.FoodRecipePrice(
                    // получаем себестоимость блюда
                    await _ingredientService.GetFoodCostPrice(food.Id));

                var info = IzumiReplyMessage.ShopRecipeFieldDesc.Parse(
                    food.Id, _emotes.GetEmoteOrBlank("Recipe"), _local.Localize(food.Name),
                    _emotes.GetEmoteOrBlank(Currency.Ien.ToString()), recipeCost,
                    _local.Localize(Currency.Ien.ToString(), recipeCost));

                if (displayString.Length + info.Length > 1024) continue;

                displayString += info;
            }

            return displayString;
        }
    }
}
