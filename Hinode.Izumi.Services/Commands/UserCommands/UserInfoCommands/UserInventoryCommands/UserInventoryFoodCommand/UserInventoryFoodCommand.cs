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
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.InventoryService.Models;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryFoodCommand
{
    [InjectableService]
    public class UserInventoryFoodCommand : IUserInventoryFoodCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IInventoryService _inventoryService;
        private readonly IImageService _imageService;

        private Dictionary<string, EmoteModel> _emotes;

        public UserInventoryFoodCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IInventoryService inventoryService, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _inventoryService = inventoryService;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            _emotes = await _emoteService.GetEmotes();
            // получаем еду пользователя
            var userFood = await _inventoryService.GetUserFood((long) context.User.Id);
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
                .WithImageUrl(await _imageService.GetImageUrl(Image.Inventory))
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

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает локализированное отображение блюда (иконка, количество, название) через запятую.
        /// </summary>
        /// <param name="userFood">Массив блюд у пользователя.</param>
        /// <returns>Локализированная строка блюда.</returns>
        private string Display(IEnumerable<UserFoodModel> userFood)
        {
            return userFood.Aggregate(string.Empty,
                (current, food) =>
                    current + (food.Amount > 0
                        ? $"{_emotes.GetEmoteOrBlank(food.Name)} {food.Amount} {_local.Localize(LocalizationCategory.Food, food.FoodId, food.Amount)}, "
                        : ""));
        }
    }
}
