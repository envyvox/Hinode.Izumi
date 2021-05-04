using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyRecipeCommand : IShopBuyRecipeCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;
        private readonly IFoodService _foodService;
        private readonly ICalculationService _calc;
        private readonly IInventoryService _inventoryService;
        private readonly IIngredientService _ingredientService;
        private readonly ILocalizationService _local;
        private readonly IMasteryService _masteryService;

        public ShopBuyRecipeCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService, IFoodService foodService, ICalculationService calc,
            IInventoryService inventoryService, IIngredientService ingredientService, ILocalizationService local,
            IMasteryService masteryService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
            _foodService = foodService;
            _calc = calc;
            _inventoryService = inventoryService;
            _ingredientService = ingredientService;
            _local = local;
            _masteryService = masteryService;
        }

        public async Task Execute(SocketCommandContext context, long foodId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // проверяем есть ли у пользователя уже этот рецепт
            var hasRecipe = await _foodService.CheckUserRecipe((long) context.User.Id, foodId);

            if (hasRecipe)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.RecipeBuyAlready.Parse(
                    emotes.GetEmoteOrBlank("Recipe"))));
            }
            else
            {
                // получаем еду этого рецепта
                var food = await _foodService.GetFood(foodId);
                // получаем мастерство кулинарии пользователя
                var userMastery = await _masteryService.GetUserMastery((long) context.User.Id, Mastery.Cooking);

                // проверяем что у пользователя достаточно мастерства
                if (food.Mastery > userMastery.Amount)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ShopRecipeMasteryWrongAmount.Parse(
                        emotes.GetEmoteOrBlank(Mastery.Cooking.ToString()), Mastery.Cooking.Localize(),
                        emotes.GetEmoteOrBlank("Recipe"))));
                }
                else
                {
                    // получаем валюту пользователя
                    var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);
                    // определяем стоимость рецепта
                    var recipeCost = await _calc.FoodRecipePrice(
                        // определяем себестоимость блюда
                        await _ingredientService.GetFoodCostPrice(food.Id));

                    // проверяем хватит ли пользователю денег на оплату рецепта
                    if (userCurrency.Amount < recipeCost)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.RecipeBuyNoCurrency.Parse(
                            emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                            _local.Localize(Currency.Ien.ToString(), 5), emotes.GetEmoteOrBlank("Recipe"))));
                    }
                    else
                    {
                        // отнимаем у пользователя деньги на оплату рецепта
                        await _inventoryService.RemoveItemFromUser(
                            (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), recipeCost);
                        // добавляем пользователю рецепт
                        await _foodService.AddRecipeToUser((long) context.User.Id, food.Id);

                        var embed = new EmbedBuilder()
                            // баннер магазина рецептов
                            .WithImageUrl(await _imageService.GetImageUrl(Image.ShopRecipe))
                            // подверждаем успешную покупку рецепта
                            .WithDescription(IzumiReplyMessage.RecipeBuySuccess.Parse(
                                emotes.GetEmoteOrBlank("Recipe"), _local.Localize(food.Name)));

                        await _discordEmbedService.SendEmbed(context.User, embed);
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
