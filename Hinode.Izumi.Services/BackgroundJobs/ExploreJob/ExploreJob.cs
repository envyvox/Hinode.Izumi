using System;
using System.Threading.Tasks;
using Discord;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.CollectionService;
using Hinode.Izumi.Services.RpgServices.FishService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Hinode.Izumi.Services.TimeService;
using Image = Hinode.Izumi.Data.Enums.Image;
using Hinode.Izumi.Services.Extensions;

namespace Hinode.Izumi.Services.BackgroundJobs.ExploreJob
{
    [InjectableService]
    public class ExploreJob : IExploreJob
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IPropertyService _propertyService;
        private readonly IImageService _imageService;
        private readonly IMasteryService _masteryService;
        private readonly ITrainingService _trainingService;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly ILocationService _locationService;
        private readonly IGatheringService _gatheringService;
        private readonly ICalculationService _calc;
        private readonly IInventoryService _inventoryService;
        private readonly ICollectionService _collectionService;
        private readonly ITimeService _timeService;
        private readonly IFishService _fishService;

        public ExploreJob(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IDiscordGuildService discordGuildService, IPropertyService propertyService,
            IImageService imageService, IMasteryService masteryService, ITrainingService trainingService,
            IStatisticService statisticService, IAchievementService achievementService, IFishService fishService,
            ILocationService locationService, IGatheringService gatheringService, ICalculationService calc,
            IInventoryService inventoryService, ICollectionService collectionService, ITimeService timeService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _discordGuildService = discordGuildService;
            _propertyService = propertyService;
            _imageService = imageService;
            _masteryService = masteryService;
            _trainingService = trainingService;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _locationService = locationService;
            _gatheringService = gatheringService;
            _calc = calc;
            _inventoryService = inventoryService;
            _collectionService = collectionService;
            _timeService = timeService;
            _fishService = fishService;
        }

        public async Task CompleteExploreGarden(long userId, long userGatheringMastery)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation(userId, Location.Garden);
            // удаляем информацию о перемещении
            await _locationService.RemoveUserMovement(userId);
            // добавляем пользователю статистку исследований сада
            await _statisticService.AddStatisticToUser(userId, Statistic.ExploreGarden);
            // проверяем выполнил ли пользователь достижения
            await _achievementService.CheckAchievement(userId, new[]
            {
                Achievement.FirstGatheringResource,
                Achievement.Gather40Resources,
                Achievement.Gather250Resources
            });

            // получаем все собирательские ресурсы этой локации
            var gatherings = await _gatheringService.GetGathering(Location.Garden);
            var gatheringsString = string.Empty;
            long itemsCount = 0;

            // теперь для каждого ресурса нужно проверить смог ли пользователь его собрать
            foreach (var gathering in gatherings)
            {
                // получаем шанс сбора этого ресурса
                var chance =
                    (await _propertyService.GetGatheringProperty(gathering.Id, GatheringProperty.GatheringChance))
                    .MasteryMaxValue(userGatheringMastery);
                // получаем шанс удвоенного сбора ресурса
                var doubleChance =
                    (await _propertyService.GetGatheringProperty(gathering.Id, GatheringProperty.GatheringDoubleChance))
                    .MasteryMaxValue(userGatheringMastery);
                // получаем количество собираемого ресурса
                var amount =
                    (await _propertyService.GetGatheringProperty(gathering.Id, GatheringProperty.GatheringAmount))
                    .MasteryMaxValue(userGatheringMastery);
                // считаем финальное количество ресурсов
                var successAmount = _calc.SuccessAmount(chance, doubleChance, amount);

                // если пользователь не смог собрать этот ресурс - пропускаем
                if (successAmount <= 0) continue;

                // добавляем ресурс пользователю
                await _inventoryService.AddItemToUser(
                    userId, InventoryCategory.Gathering, gathering.Id, successAmount);
                // добавляем ресурс в коллекцию
                await _collectionService.AddCollectionToUser(userId, CollectionCategory.Gathering, gathering.Id);
                // добавляем локализированную строку к собранным ресурсам
                gatheringsString +=
                    $"{emotes.GetEmoteOrBlank(gathering.Name)} {successAmount} {_local.Localize(gathering.Name, successAmount)}, ";
                // добавляем общее количество собранных ресурсов
                itemsCount += successAmount;
            }

            var embed = new EmbedBuilder()
                .WithAuthor(Location.ExploreGarden.Localize())
                // баннер исследования сада
                .WithImageUrl(await _imageService.GetImageUrl(Image.ExploreGarden));

            // если пользователь что-то собрал
            if (itemsCount > 0)
            {
                // добавляем пользователю мастерство сбора
                await _masteryService.AddMasteryToUser(userId, Mastery.Gathering,
                    // определяем количество полученного мастерства
                    await _calc.MasteryXp(MasteryXpProperty.Gathering, userGatheringMastery, itemsCount));
                // добавляем пользователю статистику собранных ресурсов
                await _statisticService.AddStatisticToUser(userId, Statistic.Gathering, itemsCount);

                embed.WithDescription(
                        // оповещаем о завершении исследовании сада
                        IzumiReplyMessage.ExploreForestSuccess.Parse() +
                        $"\n{emotes.GetEmoteOrBlank("Blank")}")
                    // выводим список полученных ресурсов
                    .AddField(IzumiReplyMessage.ExploreSuccessFieldName.Parse(),
                        gatheringsString.Remove(gatheringsString.Length - 2));
            }
            // если пользователь ничего не собрал
            else
            {
                embed.WithDescription(IzumiReplyMessage.ExploreForestEmpty.Parse());
            }

            await _discordEmbedService.SendEmbed(await _discordGuildService.GetSocketUser(userId), embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep(userId, TrainingStep.CompleteExploreGarden);
            // проверяем выполнил ли пользователь достижение
            await _achievementService.CheckAchievement(userId, Achievement.CompleteCollectionGathering);
        }

        public async Task CompleteExploreCastle(long userId, long userGatheringMastery)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation(userId, Location.Castle);
            // удаляем информацию о перемещении
            await _locationService.RemoveUserMovement(userId);
            // добавляем пользователю статистику исследований замка
            await _statisticService.AddStatisticToUser(userId, Statistic.ExploreCastle);
            // проверяем выполнил ли пользователь достижения
            await _achievementService.CheckAchievement(userId, new[]
            {
                Achievement.FirstGatheringResource,
                Achievement.Gather40Resources,
                Achievement.Gather250Resources
            });

            // получаем все собирательские ресурсы этой локации
            var gatherings = await _gatheringService.GetGathering(Location.Castle);
            var gatheringsString = string.Empty;
            long itemsCount = 0;

            // теперь для каждого ресурса нужно проверить смог ли пользователь его собрать
            foreach (var gathering in gatherings)
            {
                // получаем шанс сбора этого ресурса
                var chance =
                    (await _propertyService.GetGatheringProperty(gathering.Id, GatheringProperty.GatheringChance))
                    .MasteryMaxValue(userGatheringMastery);
                // получаем шанс удвоенного сбора ресурса
                var doubleChance =
                    (await _propertyService.GetGatheringProperty(gathering.Id, GatheringProperty.GatheringDoubleChance))
                    .MasteryMaxValue(userGatheringMastery);
                // получаем количество собираемого ресурса
                var amount =
                    (await _propertyService.GetGatheringProperty(gathering.Id, GatheringProperty.GatheringAmount))
                    .MasteryMaxValue(userGatheringMastery);
                // считаем финальное количество ресурсов
                var successAmount = _calc.SuccessAmount(chance, doubleChance, amount);

                // если пользователь не смог собрать этот ресурс - пропускаем
                if (successAmount <= 0) continue;

                // добавляем ресурс пользователю
                await _inventoryService.AddItemToUser(
                    userId, InventoryCategory.Gathering, gathering.Id, successAmount);
                // добавляем ресурс в коллекцию
                await _collectionService.AddCollectionToUser(userId, CollectionCategory.Gathering, gathering.Id);
                // добавляем локализированную строку к собранным ресурсам
                gatheringsString +=
                    $"{emotes.GetEmoteOrBlank(gathering.Name)} {successAmount} {_local.Localize(gathering.Name, successAmount)}, ";
                // добавляем общее количество собранных ресурсов
                itemsCount += successAmount;
            }

            var embed = new EmbedBuilder()
                .WithAuthor(Location.ExploreCastle.Localize())
                // Баннер исследования замка
                .WithImageUrl(await _imageService.GetImageUrl(Image.ExploreCastle));

            // Если пользователь что-то собрал
            if (itemsCount > 0)
            {
                // добавляем пользователю мастерство сбора
                await _masteryService.AddMasteryToUser(userId, Mastery.Gathering,
                    // определяем количество полученного мастерства
                    await _calc.MasteryXp(MasteryXpProperty.Gathering, userGatheringMastery, itemsCount));
                // добавляем пользователю статистику собранных ресурсов
                await _statisticService.AddStatisticToUser(userId, Statistic.Gathering, itemsCount);

                embed.WithDescription(
                        // оповещаем о окончании исследования замка
                        IzumiReplyMessage.ExploreCastleSuccess.Parse() +
                        $"\n{emotes.GetEmoteOrBlank("Blank")}")
                    // выводим собранные ресурсы
                    .AddField(IzumiReplyMessage.ExploreSuccessFieldName.Parse(),
                        gatheringsString.Remove(gatheringsString.Length - 2));
            }
            // Если пользователь ничего не собрал
            else
            {
                embed.WithDescription(IzumiReplyMessage.ExploreCastleEmpty.Parse());
            }

            await _discordEmbedService.SendEmbed(await _discordGuildService.GetSocketUser(userId), embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep(userId, TrainingStep.CompleteExploreCastle);
            // проверяем выполнил ли пользователь достижение
            await _achievementService.CheckAchievement(userId, Achievement.CompleteCollectionGathering);
        }

        public async Task CompleteFishing(long userId, long userFishingMastery)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем текущее время суток в мире
            var timesDay = _timeService.GetCurrentTimesDay();
            // получаем текущий сезон
            var season = (Season) await _propertyService.GetPropertyValue(Property.CurrentSeason);
            // получаем текущую погоду
            var weather = (Weather) await _propertyService.GetPropertyValue(Property.WeatherToday);

            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation(userId, Location.Seaport);
            // удаляем информацию о перемещении
            await _locationService.RemoveUserMovement(userId);
            // добавляем пользователю статистику количества рыбалок
            await _statisticService.AddStatisticToUser(userId, Statistic.Fishing);

            // определяем редкость выловленной рыбы
            var fishRarity = await _calc.FishRarity(userFishingMastery);
            // получаем случайную рыбу в этой редкости, подходящую по времени суток, сезону и погоде
            var fish = await _fishService.GetRandomFish(timesDay, season, weather, fishRarity);
            // определяем сорвалась ли рыба
            var success = await _calc.FishingCheckSuccess(userFishingMastery, fish.Rarity);

            var embed = new EmbedBuilder()
                .WithAuthor(Location.Fishing.Localize())
                // баннер рыбалки
                .WithImageUrl(await _imageService.GetImageUrl(Image.Fishing));

            // если рыба не сорвалась
            if (success)
            {
                // добавляем пользователю выловленную рыбу
                await _inventoryService.AddItemToUser(userId, InventoryCategory.Fish, fish.Id);

                // добавляем пользователю запись в коллекцию
                await _collectionService.AddCollectionToUser(userId, CollectionCategory.Fish, fish.Id);
                // проверяем выполнил ли пользователю достижения
                await _achievementService.CheckAchievement(userId, new[]
                {
                    Achievement.FirstFish,
                    Achievement.Catch50Fish,
                    Achievement.Catch300Fish
                });

                // добавляем пользователю статистику ловли рыбы по редкости
                // проверяем выполнил ли пользователь достижения на ловлю рыбы по редкости
                switch (fishRarity)
                {
                    case FishRarity.Common:
                        await _statisticService.AddStatisticToUser(userId, Statistic.FishingCommonFish);
                        break;
                    case FishRarity.Rare:
                        await _statisticService.AddStatisticToUser(userId, Statistic.FishingRareFish);
                        break;
                    case FishRarity.Epic:
                        await _statisticService.AddStatisticToUser(userId, Statistic.FishingEpicFish);
                        await _achievementService.CheckAchievement(userId, Achievement.CatchEpicFish);
                        break;
                    case FishRarity.Mythical:
                        await _statisticService.AddStatisticToUser(userId, Statistic.FishingMythical);
                        await _achievementService.CheckAchievement(userId, Achievement.CatchMythicalFish);
                        break;
                    case FishRarity.Legendary:
                        await _statisticService.AddStatisticToUser(userId, Statistic.FishingLegendary);
                        await _achievementService.CheckAchievement(userId, Achievement.CatchLegendaryFish);
                        break;
                    case FishRarity.Divine:
                        await _statisticService.AddStatisticToUser(userId, Statistic.FishingDivine);
                        await _achievementService.CheckAchievement(userId, Achievement.CatchKoi);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // оповещаем о завершении рыбалки
                embed.WithDescription(IzumiReplyMessage.FishingSuccess.Parse(
                    emotes.GetEmoteOrBlank(fish.Name), _local.Localize(fish.Name)));
            }
            // если рыба сорвалась
            else
            {
                // оповещаем о завершении рыбалки
                embed.WithDescription(IzumiReplyMessage.FishingEmpty.Parse(
                    emotes.GetEmoteOrBlank(fish.Name), _local.Localize(fish.Name)));
            }

            // добавляем пользователю мастерство рыбалки
            await _masteryService.AddMasteryToUser(userId, Mastery.Fishing,
                // определяем количество полученного мастерства рыбалки в зависимости от того, сорвалась рыба или нет
                await _calc.MasteryFishingXp(userFishingMastery, success));

            await _discordEmbedService.SendEmbed(await _discordGuildService.GetSocketUser(userId), embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep(userId, TrainingStep.CompleteFishing);
            // проверяем выполнил ли пользователь достижение
            await _achievementService.CheckAchievement(userId, Achievement.CompleteCollectionFish);
        }
    }
}
