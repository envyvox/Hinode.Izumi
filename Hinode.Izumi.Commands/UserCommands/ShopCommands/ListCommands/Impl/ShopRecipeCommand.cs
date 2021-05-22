using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopRecipeCommand : IShopRecipeCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public ShopRecipeCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            _emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущий сезон
            var season = (Season) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentSeason));
            // получаем еду из базы
            var foods = await _mediator.Send(new GetAllFoodQuery());

            var embed = new EmbedBuilder()
                // рассказываем как покупать рецепты
                .WithDescription(
                    IzumiReplyMessage.ShopRecipeDesc.Parse(
                        _emotes.GetEmoteOrBlank("Recipe")) +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина рецептов
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopRecipe)))
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

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }

        /// <summary>
        /// Заполняет строку с локализированной информацией о рецептах (иконка, стоимость, название).
        /// </summary>
        /// <param name="foods">Массив блюд.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="season">Текущий сезон.</param>
        /// <returns>Локализированная строка с информацией о рецептах.</returns>
        private async Task<string> Display(IEnumerable<FoodRecord> foods, long userId, Season season)
        {
            var displayString = string.Empty;

            // добавляем информацию о каждом рецепте
            foreach (var food in foods)
            {
                // получаем сезоны блюда
                var foodSeasons = await _mediator.Send(new GetFoodSeasonsQuery(food.Id));
                // если блюдо не содержит текущий сезон - пропускаем
                if (!foodSeasons.Contains(season)) continue;

                // проверяем есть ли этот рецепт у пользователя
                var hasRecipe = await _mediator.Send(new CheckUserHasRecipeQuery(userId, food.Id));
                // если у пользователя уже есть этот рецепт - пропускаем
                if (hasRecipe) continue;

                // определяем стоимость рецепта
                var recipePrice = await _mediator.Send(new GetFoodRecipePriceQuery(
                    // получаем себестоимость блюда
                    await _mediator.Send(new GetFoodCostPriceQuery(food.Id))));

                var info = IzumiReplyMessage.ShopRecipeFieldDesc.Parse(
                    food.Id, _emotes.GetEmoteOrBlank("Recipe"), _local.Localize(food.Name, 2),
                    _emotes.GetEmoteOrBlank(Currency.Ien.ToString()), recipePrice,
                    _local.Localize(Currency.Ien.ToString(), recipePrice));

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
