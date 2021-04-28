using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.EmoteService.Models;
using Hinode.Izumi.Services.RpgServices.FishService;
using Hinode.Izumi.Services.RpgServices.FishService.Models;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.FisherCommands.FisherListCommand
{
    [InjectableService]
    public class FisherListCommand : IFisherListCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IPropertyService _propertyService;
        private readonly IFishService _fishService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _local;

        private Dictionary<string, EmoteModel> _emotes;

        public FisherListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IPropertyService propertyService, IFishService fishService, IImageService imageService,
            ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _propertyService = propertyService;
            _fishService = fishService;
            _imageService = imageService;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            _emotes = await _emoteService.GetEmotes();
            // получаем текущий сезон в мире
            var season = (Season) await _propertyService.GetPropertyValue(Property.CurrentSeason);
            // получаем всю рыбу текущего сезона
            var fish = await _fishService.GetAllFish(season);
            // разбиваем ее по редкости
            var commonFish = fish.Where(x => x.Rarity == FishRarity.Common);
            var rareFish = fish.Where(x => x.Rarity == FishRarity.Rare);
            var epicFish = fish.Where(x => x.Rarity == FishRarity.Epic);
            var mythicalFish = fish.Where(x => x.Rarity == FishRarity.Mythical);
            var legendaryFish = fish.Where(x => x.Rarity == FishRarity.Legendary);

            var embed = new EmbedBuilder()
                // рассказываем как продавать рыбу
                .WithDescription(
                    IzumiReplyMessage.FisherShopDesc.Parse() +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина рыбака
                .WithImageUrl(await _imageService.GetImageUrl(Image.ShopFisher))
                // заполняем embed field рыбой и ее стоимостью
                .AddField(FishRarity.Common.Localize(), Display(commonFish))
                .AddField(FishRarity.Rare.Localize(), Display(rareFish))
                .AddField(FishRarity.Epic.Localize(), Display(epicFish))
                .AddField(FishRarity.Mythical.Localize(), Display(mythicalFish))
                .AddField(FishRarity.Legendary.Localize(), Display(legendaryFish));

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает локализированную строку рыбы (иконка, название, стоимость).
        /// </summary>
        /// <param name="model">Массив рыбы.</param>
        /// <returns>Локализированную строку рыбы.</returns>
        private string Display(IEnumerable<FishModel> model)
        {
            return model
                .Aggregate(string.Empty,
                    (current, fish) =>
                        current + IzumiReplyMessage.FisherShopFishDesc.Parse(
                            _emotes.GetEmoteOrBlank("List"), fish.Id, _emotes.GetEmoteOrBlank(fish.Name),
                            _local.Localize(fish.Name), _emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                            fish.Price, _local.Localize(Currency.Ien.ToString(), fish.Price)));
        }
    }
}
