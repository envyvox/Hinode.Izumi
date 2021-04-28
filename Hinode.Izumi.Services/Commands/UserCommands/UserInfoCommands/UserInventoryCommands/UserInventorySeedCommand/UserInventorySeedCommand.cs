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

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventorySeedCommand
{
    [InjectableService]
    public class UserInventorySeedCommand : IUserInventorySeedCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IInventoryService _inventoryService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _local;

        private Dictionary<string, EmoteModel> _emotes;

        public UserInventorySeedCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IInventoryService inventoryService, IImageService imageService, ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _inventoryService = inventoryService;
            _imageService = imageService;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            _emotes = await _emoteService.GetEmotes();
            // получаем семена пользователя
            var userSeeds = await _inventoryService.GetUserSeed((long) context.User.Id);
            // разбиваем их по сезонам
            var springSeeds = userSeeds.Where(x => x.Season == Season.Spring);
            var summerSeeds = userSeeds.Where(x => x.Season == Season.Summer);
            var autumnSeeds = userSeeds.Where(x => x.Season == Season.Autumn);

            var springSeedsString = Display(springSeeds);
            var summerSeedsString = Display(summerSeeds);
            var autumnSeedsString = Display(autumnSeeds);

            var embed = new EmbedBuilder()
                // баннер инвентаря
                .WithImageUrl(await _imageService.GetImageUrl(Image.Inventory))
                .WithDescription(IzumiReplyMessage.UserSeedsDesc.Parse())
                // весенние семена
                .AddField(IzumiReplyMessage.UserSeedsSpringFieldName.Parse(),
                    springSeedsString.Length > 0
                        ? springSeedsString.Remove(springSeedsString.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // летние семена
                .AddField(IzumiReplyMessage.UserSeedsSummerFieldName.Parse(),
                    summerSeedsString.Length > 0
                        ? summerSeedsString.Remove(summerSeedsString.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // осенние семена
                .AddField(IzumiReplyMessage.UserSeedsAutumnFieldName.Parse(),
                    autumnSeedsString.Length > 0
                        ? autumnSeedsString.Remove(autumnSeedsString.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse());

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает локализированное отображение семени (иконка, количество, название) через запятую.
        /// </summary>
        /// <param name="seedInUser">Массив моделей семени у пользователя.</param>
        /// <returns>Локализированная строка семени.</returns>
        private string Display(IEnumerable<UserSeedModel> seedInUser)
        {
            return seedInUser
                .Aggregate(string.Empty,
                    (current, seed) =>
                        current + (seed.Amount > 0
                            ? $"{_emotes.GetEmoteOrBlank(seed.Name)} {seed.Amount} {_local.Localize(seed.Name, seed.Amount)}, "
                            : ""));
        }
    }
}
