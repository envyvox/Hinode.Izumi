using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Commands;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyRecipeCommand : IShopBuyRecipeCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBuyRecipeCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long foodId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // проверяем есть ли у пользователя уже этот рецепт
            var hasRecipe = await _mediator.Send(new CheckUserHasRecipeQuery((long) context.User.Id, foodId));

            if (hasRecipe)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.RecipeBuyAlready.Parse(
                    emotes.GetEmoteOrBlank("Recipe"))));
            }
            else
            {
                // получаем еду этого рецепта
                var food = await _mediator.Send(new GetFoodQuery(foodId));
                // получаем мастерство кулинарии пользователя
                var userMastery = await _mediator.Send(new GetUserMasteryQuery(
                    (long) context.User.Id, Mastery.Cooking));

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
                    var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(
                        (long) context.User.Id, Currency.Ien));
                    // определяем стоимость рецепта
                    var recipeCost = await _mediator.Send(new GetFoodRecipePriceQuery(
                        // определяем себестоимость блюда
                        await _mediator.Send(new GetFoodCostPriceQuery(food.Id))));

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
                        await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                            (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                            recipeCost));
                        // добавляем пользователю рецепт
                        await _mediator.Send(new AddRecipeToUserCommand((long) context.User.Id, food.Id));

                        var embed = new EmbedBuilder()
                            // баннер магазина рецептов
                            .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopRecipe)))
                            // подверждаем успешную покупку рецепта
                            .WithDescription(IzumiReplyMessage.RecipeBuySuccess.Parse(
                                emotes.GetEmoteOrBlank("Recipe"), _local.Localize(food.Name)));

                        await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
