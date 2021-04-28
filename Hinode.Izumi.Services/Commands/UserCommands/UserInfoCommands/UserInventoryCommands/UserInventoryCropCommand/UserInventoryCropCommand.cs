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

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCropCommand
{
    [InjectableService]
    public class UserInventoryCropCommand : IUserInventoryCropCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IInventoryService _inventoryService;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;

        private Dictionary<string, EmoteModel> _emotes;

        public UserInventoryCropCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IInventoryService inventoryService, ILocalizationService local, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _inventoryService = inventoryService;
            _local = local;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            _emotes = await _emoteService.GetEmotes();
            // получаем урожай пользователя
            var userCrops = await _inventoryService.GetUserCrop((long) context.User.Id);
            // разбиваем его сезоны
            var springCrops = userCrops.Where(x => x.Season == Season.Spring);
            var summerCrops = userCrops.Where(x => x.Season == Season.Summer);
            var autumnCrops = userCrops.Where(x => x.Season == Season.Autumn);

            var springCropsString = Display(springCrops);
            var summerCropsString = Display(summerCrops);
            var autumnCropsString = Display(autumnCrops);

            var embed = new EmbedBuilder()
                // баннер инвентаря
                .WithImageUrl(await _imageService.GetImageUrl(Image.Inventory))
                .WithDescription(IzumiReplyMessage.UserCropsDesc.Parse())
                // весенний урожай
                .AddField(IzumiReplyMessage.UserCropsSpringFieldName.Parse(),
                    springCropsString.Length > 0
                        ? springCropsString.Remove(springCropsString.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // летний урожай
                .AddField(IzumiReplyMessage.UserCropsSummerFieldName.Parse(),
                    summerCropsString.Length > 0
                        ? summerCropsString.Remove(summerCropsString.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // осенний урожай
                .AddField(IzumiReplyMessage.UserCropsAutumnFieldName.Parse(),
                    autumnCropsString.Length > 0
                        ? autumnCropsString.Remove(autumnCropsString.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse());

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает локализированное отображение урожая (иконка, количество, название) через запятую.
        /// </summary>
        /// <param name="cropInUser">Массив урожая у пользователя.</param>
        /// <returns>Локализированная строка урожая.</returns>
        private string Display(IEnumerable<UserCropModel> cropInUser)
        {
            return cropInUser
                .Aggregate(string.Empty,
                    (current, crop) =>
                        current + (crop.Amount > 0
                            ? $"{_emotes.GetEmoteOrBlank(crop.Name)} {crop.Amount} {_local.Localize(crop.Name, crop.Amount)}, "
                            : ""));
        }
    }
}
