using System;
using System.Collections.Generic;
using System.Globalization;
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
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CookingCommands.CookingListCommand
{
    [InjectableService]
    public class CookingListCommand : ICookingListCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IFoodService _foodService;
        private readonly IIngredientService _ingredientService;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;
        private readonly ICalculationService _calc;
        private readonly ITrainingService _trainingService;

        private Dictionary<string, EmoteModel> _emotes;

        public CookingListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IFoodService foodService, IIngredientService ingredientService, ILocalizationService local,
            IImageService imageService, ICalculationService calc, ITrainingService trainingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _foodService = foodService;
            _ingredientService = ingredientService;
            _local = local;
            _imageService = imageService;
            _calc = calc;
            _trainingService = trainingService;
        }

        public async Task Execute(SocketCommandContext context, long masteryBracket)
        {
            // получаем иконки из базы
            _emotes = await _emoteService.GetEmotes();

            var embed = new EmbedBuilder()
                // баннер приготовления
                .WithImageUrl(await _imageService.GetImageUrl(Image.Cooking));

            // если пользователь не указал категорию - выводим список категорий
            if (masteryBracket == 0)
            {
                embed.WithDescription(IzumiReplyMessage.CookingListCategoryDesc.Parse())
                    .AddField(IzumiReplyMessage.CookingListCategoryFieldName.Parse(),
                        IzumiReplyMessage.CookingListCategoryFieldDesc.Parse(
                            _emotes.GetEmoteOrBlank("List"),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery0.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery50.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery100.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery150.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery200.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery250.Parse()));
            }
            else
            {
                // получаем рецепты пользователя
                var userRecipes = await _foodService.GetUserRecipes((long) context.User.Id);

                // если у пользователя нет рецептов - рассказываем как их купить
                if (userRecipes.Length < 1)
                {
                    embed.WithDescription(IzumiReplyMessage.CookingListNull.Parse(
                        Location.Garden.Localize(true)));
                }
                else
                {
                    // рассказываем как приготовить блюдо
                    embed.WithDescription(
                        IzumiReplyMessage.CookingListDesc.Parse() +
                        $"\n{_emotes.GetEmoteOrBlank("Blank")}");

                    // определяем какое мастерство ввел пользователь
                    long foodMastery = 0;
                    switch (masteryBracket)
                    {
                        case 1:
                            foodMastery = 0;
                            break;
                        case 2:
                            foodMastery = 50;
                            break;
                        case 3:
                            foodMastery = 100;
                            break;
                        case 4:
                            foodMastery = 150;
                            break;
                        case 5:
                            foodMastery = 200;
                            break;
                        case 6:
                            foodMastery = 250;
                            break;
                        default:
                            await Task.FromException(new Exception(IzumiReplyMessage.CookingListCategoryWrong.Parse()));
                            break;
                    }

                    // для каждого рецепта создаем embed field
                    foreach (var food in userRecipes.Where(x => x.Mastery == foodMastery))
                    {
                        // получаем стоимость приготовления
                        var cookingPrice = await _calc.CraftingPrice(
                            await _ingredientService.GetFoodCostPrice(food.Id));
                        // получаем локализированную строку ингредиентов
                        var ingredients = await _ingredientService.DisplayFoodIngredients(food.Id);

                        embed.AddField(
                            // выводим иконку и название еду
                            IzumiReplyMessage.CookingListFieldName.Parse(
                                food.Id, _emotes.GetEmoteOrBlank(food.Name), _local.Localize(food.Name, 5)),
                            // выводим информацию о приготовлении еды
                            IzumiReplyMessage.CookingListFieldDesc.Parse(
                                ingredients, _emotes.GetEmoteOrBlank(Currency.Ien.ToString()), cookingPrice,
                                _local.Localize(Currency.Ien.ToString(), cookingPrice),
                                food.Time.Seconds().Humanize(2, new CultureInfo("ru-RU"))));
                    }
                }
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) context.User.Id, TrainingStep.CheckCookingList);
            await Task.CompletedTask;
        }
    }
}
