using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryFoodCommand
{
    [InjectableService]
    public class UserInventoryFoodCommand : IUserInventoryFoodCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public UserInventoryFoodCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            _emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем еду пользователя
            var userFood = await _mediator.Send(new GetUserFoodsQuery((long) context.User.Id));
            // разбиваем ее по мастерству
            var foodMastery0 = userFood.Where(x => x.Mastery == 0);
            var foodMastery50 = userFood.Where(x => x.Mastery == 50);
            var foodMastery100 = userFood.Where(x => x.Mastery == 100);
            var foodMastery150 = userFood.Where(x => x.Mastery == 150);
            var foodMastery200 = userFood.Where(x => x.Mastery == 200);
            var foodMastery250 = userFood.Where(x => x.Mastery == 250);

            var foodMastery0String = Display(foodMastery0);
            var foodMastery50String = Display(foodMastery50);
            var foodMastery100String = Display(foodMastery100);
            var foodMastery150String = Display(foodMastery150);
            var foodMastery200String = Display(foodMastery200);
            var foodMastery250String = Display(foodMastery250);

            var embed = new EmbedBuilder()
                // баннер инвентаря
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Inventory)))
                .WithDescription(IzumiReplyMessage.UserFoodDesc.Parse())
                // блюда начинающего повара
                .AddField(IzumiReplyMessage.UserFoodMastery0.Parse(),
                    foodMastery0String.Length > 0
                        ? foodMastery0String.Remove(foodMastery0String.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // блюда повара-ученика
                .AddField(IzumiReplyMessage.UserFoodMastery50.Parse(),
                    foodMastery50String.Length > 0
                        ? foodMastery50String.Remove(foodMastery50String.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // блюда опытного повара
                .AddField(IzumiReplyMessage.UserFoodMastery100.Parse(),
                    foodMastery100String.Length > 0
                        ? foodMastery100String.Remove(foodMastery100String.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // блюда повара-профессионала
                .AddField(IzumiReplyMessage.UserFoodMastery150.Parse(),
                    foodMastery150String.Length > 0
                        ? foodMastery150String.Remove(foodMastery150String.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // блюда повара-эксперта
                .AddField(IzumiReplyMessage.UserFoodMastery200.Parse(),
                    foodMastery200String.Length > 0
                        ? foodMastery200String.Remove(foodMastery200String.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // блюда мастера-повара
                .AddField(IzumiReplyMessage.UserFoodMastery250.Parse(),
                    foodMastery250String.Length > 0
                        ? foodMastery250String.Remove(foodMastery250String.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse());

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает локализированное отображение блюда (иконка, количество, название) через запятую.
        /// </summary>
        /// <param name="userFood">Массив блюд у пользователя.</param>
        /// <returns>Локализированная строка блюда.</returns>
        private string Display(IEnumerable<UserFoodRecord> userFood)
        {
            return userFood.Aggregate(string.Empty,
                (current, food) =>
                    current + (food.Amount > 0
                        ? $"{_emotes.GetEmoteOrBlank(food.Name)} {food.Amount} {_local.Localize(LocalizationCategory.Food, food.FoodId, food.Amount)}, "
                        : ""));
        }
    }
}
