using System;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.CollectionService.Commands;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.MasteryService.Commands;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.HangfireJobService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Hinode.Izumi.Services.TimeService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.ExploreJob
{
    [InjectableService]
    public class ExploreJob : IExploreJob
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ExploreJob(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task CompleteExploreGarden(long userId, long userGatheringMastery)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущее событие
            var currentEvent = (Event) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentEvent));
            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, Location.Garden));
            // удаляем информацию о перемещении
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            // добавляем пользователю статистку исследований сада
            await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.ExploreGarden));
            // проверяем выполнил ли пользователь достижения
            await _mediator.Send(new CheckAchievementsInUserCommand(userId, new[]
            {
                Achievement.FirstGatheringResource,
                Achievement.Gather40Resources,
                Achievement.Gather250Resources
            }));

            // получаем все собирательские ресурсы этой локации
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(Location.Garden,
                // добавляя в пул предметы события, если такие есть
                currentEvent));
            var gatheringsString = string.Empty;
            long itemsCount = 0;

            // теперь для каждого ресурса нужно проверить смог ли пользователь его собрать
            foreach (var gathering in gatherings)
            {
                // получаем шанс сбора этого ресурса
                var chance = (await _mediator.Send(new GetGatheringPropertiesQuery(
                        gathering.Id, GatheringProperty.GatheringChance)))
                    .MasteryMaxValue(userGatheringMastery);
                // получаем шанс удвоенного сбора ресурса
                var doubleChance = (await _mediator.Send(new GetGatheringPropertiesQuery(
                        gathering.Id, GatheringProperty.GatheringDoubleChance)))
                    .MasteryMaxValue(userGatheringMastery);
                // получаем количество собираемого ресурса
                var amount = (await _mediator.Send(new GetGatheringPropertiesQuery(
                        gathering.Id, GatheringProperty.GatheringAmount)))
                    .MasteryMaxValue(userGatheringMastery);
                // считаем финальное количество ресурсов
                var successAmount = await _mediator.Send(new GetSuccessAmountQuery(chance, doubleChance, amount));

                // если пользователь не смог собрать этот ресурс - пропускаем
                if (successAmount <= 0) continue;

                // добавляем ресурс пользователю
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    userId, InventoryCategory.Gathering, gathering.Id, successAmount));
                // добавляем ресурс в коллекцию
                await _mediator.Send(new AddCollectionToUserCommand(
                    userId, CollectionCategory.Gathering, gathering.Id));
                // добавляем локализированную строку к собранным ресурсам
                gatheringsString +=
                    $"{emotes.GetEmoteOrBlank(gathering.Name)} {successAmount} {_local.Localize(gathering.Name, successAmount)}, ";
                // добавляем общее количество собранных ресурсов
                itemsCount += successAmount;
            }

            var embed = new EmbedBuilder()
                .WithAuthor(Location.ExploreGarden.Localize())
                // баннер исследования сада
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ExploreGarden)));

            // если пользователь что-то собрал
            if (itemsCount > 0)
            {
                // добавляем пользователю мастерство сбора
                await _mediator.Send(new AddMasteryToUserCommand(userId, Mastery.Gathering,
                    // определяем количество полученного мастерства
                    await _mediator.Send(new GetMasteryXpQuery(
                        MasteryXpProperty.Gathering, userGatheringMastery, itemsCount))));
                // добавляем пользователю статистику собранных ресурсов
                await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.Gathering, itemsCount));

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

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStep.CompleteExploreGarden));
            // проверяем выполнил ли пользователь достижение
            await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CompleteCollectionGathering));
            // проверяем выполнил ли пользователь достижение события
            if (currentEvent == Event.June)
            {
                await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.MeetSummer));
            }
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireAction.Explore));
        }

        public async Task CompleteExploreCastle(long userId, long userGatheringMastery)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, Location.Castle));
            // удаляем информацию о перемещении
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            // добавляем пользователю статистику исследований замка
            await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.ExploreCastle));
            // проверяем выполнил ли пользователь достижения
            await _mediator.Send(new CheckAchievementsInUserCommand(userId, new[]
            {
                Achievement.FirstGatheringResource,
                Achievement.Gather40Resources,
                Achievement.Gather250Resources
            }));

            // получаем все собирательские ресурсы этой локации
            var gatherings = await _mediator.Send(new GetGatheringsInLocationQuery(Location.Castle,
                // добавляя в пул предметы события, если такие есть
                (Event) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentEvent))));
            var gatheringsString = string.Empty;
            long itemsCount = 0;

            // теперь для каждого ресурса нужно проверить смог ли пользователь его собрать
            foreach (var gathering in gatherings)
            {
                // получаем шанс сбора этого ресурса
                var chance = (await _mediator.Send(new GetGatheringPropertiesQuery(
                        gathering.Id, GatheringProperty.GatheringChance)))
                    .MasteryMaxValue(userGatheringMastery);
                // получаем шанс удвоенного сбора ресурса
                var doubleChance = (await _mediator.Send(new GetGatheringPropertiesQuery(
                        gathering.Id, GatheringProperty.GatheringDoubleChance)))
                    .MasteryMaxValue(userGatheringMastery);
                // получаем количество собираемого ресурса
                var amount = (await _mediator.Send(new GetGatheringPropertiesQuery(
                        gathering.Id, GatheringProperty.GatheringAmount)))
                    .MasteryMaxValue(userGatheringMastery);
                // считаем финальное количество ресурсов
                var successAmount = await _mediator.Send(new GetSuccessAmountQuery(chance, doubleChance, amount));

                // если пользователь не смог собрать этот ресурс - пропускаем
                if (successAmount <= 0) continue;

                // добавляем ресурс пользователю
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    userId, InventoryCategory.Gathering, gathering.Id, successAmount));
                // добавляем ресурс в коллекцию
                await _mediator.Send(new AddCollectionToUserCommand(
                    userId, CollectionCategory.Gathering, gathering.Id));
                // добавляем локализированную строку к собранным ресурсам
                gatheringsString +=
                    $"{emotes.GetEmoteOrBlank(gathering.Name)} {successAmount} {_local.Localize(gathering.Name, successAmount)}, ";
                // добавляем общее количество собранных ресурсов
                itemsCount += successAmount;
            }

            var embed = new EmbedBuilder()
                .WithAuthor(Location.ExploreCastle.Localize())
                // Баннер исследования замка
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ExploreCastle)));

            // Если пользователь что-то собрал
            if (itemsCount > 0)
            {
                // добавляем пользователю мастерство сбора
                await _mediator.Send(new AddMasteryToUserCommand(userId, Mastery.Gathering,
                    // определяем количество полученного мастерства
                    await _mediator.Send(new GetMasteryXpQuery(
                        MasteryXpProperty.Gathering, userGatheringMastery, itemsCount))));
                // добавляем пользователю статистику собранных ресурсов
                await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.Gathering, itemsCount));

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

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStep.CompleteExploreCastle));
            // проверяем выполнил ли пользователь достижение
            await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CompleteCollectionGathering));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireAction.Explore));
        }

        public async Task CompleteFishing(long userId, long userFishingMastery)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущее время суток в мире
            var timesDay = await _mediator.Send(new GetCurrentTimesDayQuery());
            // получаем текущий сезон
            var season = (Season) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentSeason));
            // получаем текущую погоду
            var weather = (Weather) await _mediator.Send(new GetPropertyValueQuery(Property.WeatherToday));

            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, Location.Seaport));
            // удаляем информацию о перемещении
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            // добавляем пользователю статистику количества рыбалок
            await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.Fishing));

            // определяем редкость выловленной рыбы
            var fishRarity = await _mediator.Send(new GetRandomFishRarityQuery(userFishingMastery));
            // получаем случайную рыбу в этой редкости, подходящую по времени суток, сезону и погоде
            var fish = await _mediator.Send(new GetRandomFishWithParamsQuery(timesDay, season, weather, fishRarity));
            // определяем сорвалась ли рыба
            var success = await _mediator.Send(new CheckFishingSuccessQuery(userFishingMastery, fish.Rarity));

            var embed = new EmbedBuilder()
                .WithAuthor(Location.Fishing.Localize())
                // баннер рыбалки
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Fishing)));

            // если рыба не сорвалась
            if (success)
            {
                // добавляем пользователю выловленную рыбу
                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                    userId, InventoryCategory.Fish, fish.Id));
                // добавляем пользователю запись в коллекцию
                await _mediator.Send(new AddCollectionToUserCommand(userId, CollectionCategory.Fish, fish.Id));
                // проверяем выполнил ли пользователю достижения
                await _mediator.Send(new CheckAchievementsInUserCommand(userId, new[]
                {
                    Achievement.FirstFish,
                    Achievement.Catch50Fish,
                    Achievement.Catch300Fish
                }));

                // добавляем пользователю статистику ловли рыбы по редкости
                // проверяем выполнил ли пользователь достижения на ловлю рыбы по редкости
                switch (fishRarity)
                {
                    case FishRarity.Common:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.FishingCommonFish));
                        break;
                    case FishRarity.Rare:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.FishingRareFish));
                        break;
                    case FishRarity.Epic:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.FishingEpicFish));
                        await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CatchEpicFish));
                        break;
                    case FishRarity.Mythical:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.FishingMythical));
                        await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CatchMythicalFish));
                        break;
                    case FishRarity.Legendary:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.FishingLegendary));
                        await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CatchLegendaryFish));
                        break;
                    case FishRarity.Divine:
                        await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.FishingDivine));
                        await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CatchKoi));
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
            await _mediator.Send(new AddMasteryToUserCommand(userId, Mastery.Fishing,
                // определяем количество полученного мастерства рыбалки в зависимости от того, сорвалась рыба или нет
                await _mediator.Send(new GetMasteryFishingXpQuery(userFishingMastery, success))));

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStep.CompleteFishing));
            // проверяем выполнил ли пользователь достижение
            await _mediator.Send(new CheckAchievementInUserCommand(userId, Achievement.CompleteCollectionFish));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireAction.Explore));
        }
    }
}
