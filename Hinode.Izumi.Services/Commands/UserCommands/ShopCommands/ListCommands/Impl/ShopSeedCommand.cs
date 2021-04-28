using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.CropService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.SeedService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Humanizer;
using Humanizer.Localisation;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopSeedCommand : IShopSeedCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IPropertyService _propertyService;
        private readonly ITrainingService _trainingService;
        private readonly ICalculationService _calc;
        private readonly ISeedService _seedService;
        private readonly ICropService _cropService;
        private readonly IMasteryService _masteryService;
        private readonly IImageService _imageService;

        public ShopSeedCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IPropertyService propertyService, ITrainingService trainingService,
            ICalculationService calc, ISeedService seedService, ICropService cropService,
            IMasteryService masteryService, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _propertyService = propertyService;
            _trainingService = trainingService;
            _calc = calc;
            _seedService = seedService;
            _cropService = cropService;
            _masteryService = masteryService;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем текущий сезон
            var season = (Season) await _propertyService.GetPropertyValue(Property.CurrentSeason);
            // получаем семена текущего сезона
            var seeds = await _seedService.GetSeed(season);
            // получаем мастерство торговли пользователя
            var userMastery = await _masteryService.GetUserMastery((long) context.User.Id, Mastery.Trading);

            var embed = new EmbedBuilder()
                // рассказываем как покупать семена
                .WithDescription(
                    IzumiReplyMessage.CapitalSeedShopDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина семян
                .WithImageUrl(await _imageService.GetImageUrl(Image.ShopSeed));

            // для каждого семени создаем embed field
            foreach (var seed in seeds)
            {
                // получаем урожай из этого семени
                var crop = await _cropService.GetCropBySeedId(seed.Id);
                // определяем стоимость семени
                var seedPrice = await _calc.SeedPriceWithDiscount(
                    // округяем мастерство пользователя
                    (long) Math.Floor(userMastery.Amount), seed.Price);

                // заполняем информацию о семени
                var seedDesc = IzumiReplyMessage.CapitalSeedShopSeedDesc.Parse(
                    seed.Growth.Days().Humanize(1, new CultureInfo("ru-RU"), TimeUnit.Day),
                    emotes.GetEmoteOrBlank(crop.Name), _local.Localize(crop.Name),
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), crop.Price,
                    _local.Localize(Currency.Ien.ToString(), crop.Price));

                // если семя имеет возможность давать несколько плодов из одного семени - добавляем строку об этом
                if (seed.Multiply)
                    seedDesc += IzumiReplyMessage.CapitalSeedShopSeedMultiple.Parse(emotes.GetEmoteOrBlank("List"));
                // если семя имеет возможность повторного роста - добавляем строку об этом
                if (seed.ReGrowth > 0)
                    seedDesc += IzumiReplyMessage.CapitalSeedShopSeedReGrowth.Parse(emotes.GetEmoteOrBlank("List"),
                        seed.ReGrowth.Days().Humanize(1, new CultureInfo("ru-RU"), TimeUnit.Day));

                // выводим информацию о семени
                embed.AddField(IzumiReplyMessage.CapitalSeedShopSeedFieldName.Parse(
                    emotes.GetEmoteOrBlank("List"), seed.Id, emotes.GetEmoteOrBlank(seed.Name),
                    _local.Localize(seed.Name), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                    // если цена по-умолчанию отличается от финальной стоимости, выводим строку с отображением скидки
                    seed.Price != seedPrice
                        ? $"~~{seed.Price}~~ {seedPrice} {_local.Localize(Currency.Ien.ToString(), seedPrice)}"
                        : $"{seed.Price} {_local.Localize(Currency.Ien.ToString(), seed.Price)}"
                ), seedDesc + $"{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) context.User.Id, TrainingStep.CheckCapitalSeedShop);
            await Task.CompletedTask;
        }
    }
}
