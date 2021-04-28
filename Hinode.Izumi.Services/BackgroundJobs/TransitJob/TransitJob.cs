using System;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.TransitJob
{
    [InjectableService]
    public class TransitJob : ITransitJob
    {
        private readonly ILocationService _locationService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly ITrainingService _trainingService;
        private readonly IEmoteService _emoteService;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly IImageService _imageService;

        public TransitJob(ILocationService locationService, IDiscordGuildService discordGuildService,
            IDiscordEmbedService discordEmbedService, ITrainingService trainingService, IEmoteService emoteService,
            IStatisticService statisticService, IAchievementService achievementService, IImageService imageService)
        {
            _locationService = locationService;
            _discordGuildService = discordGuildService;
            _discordEmbedService = discordEmbedService;
            _trainingService = trainingService;
            _emoteService = emoteService;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _imageService = imageService;
        }

        public async Task CompleteTransit(long userId, Location destination)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();

            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation(userId, destination);
            // удаляем информацию о перемещении
            await _locationService.RemoveUserMovement(userId);

            var embed = new EmbedBuilder()
                // баннер перемещения
                .WithImageUrl(await _imageService.GetImageUrl(Image.InTransit))
                .WithDescription(
                    // оповещаем о завершении перемещения
                    IzumiReplyMessage.TransitCompleteNotify.Parse(
                        destination.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // если перемещение было не в подлокацию
            if (!destination.SubLocation())
            {
                // снимаем с пользователя роль "в пути" в дискорде
                await _discordGuildService.ToggleRoleInUser(userId, DiscordRole.LocationInTransit, false);
                // добавляем пользователю роль новой локации в дискорде
                await _discordGuildService.ToggleRoleInUser(userId,
                    // определяем роль дискорда по локации
                    _locationService.GetLocationRole(destination), true);
                // добавляем пользователю статистику перемещений
                await _statisticService.AddStatisticToUser(userId, Statistic.Transit);
                // проверяем выполнил ли пользователь достижение
                await _achievementService.CheckAchievement(userId, Achievement.FirstTransit);

                // получаем список каналов дискорда из базы
                var channels = await _discordGuildService.GetChannels();
                DiscordChannel descChannel;
                DiscordChannel whatToDoChannel;
                DiscordChannel eventsChannel;

                // получаем каналы категории города
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (destination)
                {
                    case Location.Capital:
                        descChannel = DiscordChannel.CapitalDesc;
                        whatToDoChannel = DiscordChannel.CapitalWhatToDo;
                        eventsChannel = DiscordChannel.CapitalEvents;
                        break;
                    case Location.Garden:
                        descChannel = DiscordChannel.GardenDesc;
                        whatToDoChannel = DiscordChannel.GardenWhatToDo;
                        eventsChannel = DiscordChannel.GardenEvents;
                        break;
                    case Location.Seaport:
                        descChannel = DiscordChannel.SeaportDesc;
                        whatToDoChannel = DiscordChannel.SeaportWhatToDo;
                        eventsChannel = DiscordChannel.SeaportEvents;
                        break;
                    case Location.Castle:
                        descChannel = DiscordChannel.CastleDesc;
                        whatToDoChannel = DiscordChannel.CastleWhatToDo;
                        eventsChannel = DiscordChannel.CastleEvents;
                        break;
                    case Location.Village:
                        descChannel = DiscordChannel.VillageDesc;
                        whatToDoChannel = DiscordChannel.VillageWhatToDo;
                        eventsChannel = DiscordChannel.VillageEvents;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(destination), destination, null);
                }


                // добавляем embed field с описанием каналов в этой категории
                embed.AddField(IzumiReplyMessage.TransitCompleteInfoChannelsFieldName.Parse(),
                    $"<#{channels[descChannel].Id}>, <#{channels[whatToDoChannel].Id}>, <#{channels[eventsChannel].Id}>");
            }

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId), embed);

            // проверяем нужно ли двинуть прогресс обучения пользователя
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (destination)
            {
                case Location.Seaport:
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToSeaport);
                    break;
                case Location.Garden:
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToGarden);
                    break;
                case Location.Castle:
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToCastle);
                    break;
                case Location.Village:
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToVillage);
                    break;
                case Location.Capital:
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToCapital);
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToCapitalAfterSeedShop);
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToCapitalAfterMarket);
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToCapitalAfterCasino);
                    break;
                case Location.CapitalShop:
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToCapitalShop);
                    break;
                case Location.CapitalMarket:
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToCapitalMarket);
                    break;
                case Location.CapitalCasino:
                    await _trainingService.CheckStep(userId, TrainingStep.TransitToCapitalCasino);
                    break;
            }
        }
    }
}
