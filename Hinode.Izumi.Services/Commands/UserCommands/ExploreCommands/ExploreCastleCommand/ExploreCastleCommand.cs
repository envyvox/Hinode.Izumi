using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.ExploreJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ExploreCommands.ExploreCastleCommand
{
    [InjectableService]
    public class ExploreCastleCommand : IExploreCastleCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IMasteryService _masteryService;
        private readonly ICalculationService _calc;
        private readonly ILocationService _locationService;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;
        private readonly IGatheringService _gatheringService;

        public ExploreCastleCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IMasteryService masteryService, ICalculationService calc, ILocationService locationService,
            IUserService userService, IPropertyService propertyService, TimeZoneInfo timeZoneInfo,
            ILocalizationService local, IImageService imageService, IGatheringService gatheringService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _masteryService = masteryService;
            _calc = calc;
            _locationService = locationService;
            _userService = userService;
            _propertyService = propertyService;
            _timeZoneInfo = timeZoneInfo;
            _local = local;
            _imageService = imageService;
            _gatheringService = gatheringService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем текущее время
            var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo);
            // получаем пользователя из базы
            var user = await _userService.GetUser((long) context.User.Id);
            // получаем мастерство сбора пользователя
            var userMastery = await _masteryService.GetUserMastery((long) context.User.Id, Mastery.Gathering);
            // округляем мастерство
            var masteryAmount = (long) Math.Floor(userMastery.Amount);
            // определяем длительность исследования
            var exploreTime = _calc.ActionTime(await _calc.GatheringTime(masteryAmount), user.Energy);
            // получаем собирательские ресурсы этой локации
            var gatherings = await _gatheringService.GetGathering(Location.Castle);

            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation((long) context.User.Id, Location.ExploreCastle);
            // добавляем информацию о перемещении
            await _locationService.AddUserMovement((long) context.User.Id,
                Location.ExploreCastle, Location.Castle, timeNow.AddMinutes(exploreTime));
            // отнимаем энергию у пользователя
            await _userService.RemoveEnergyFromUser((long) context.User.Id,
                await _propertyService.GetPropertyValue(Property.EnergyCostExplore));

            // запускаем джобу для окончания исследования
            BackgroundJob.Schedule<IExploreJob>(x =>
                    x.CompleteExploreCastle((long) context.User.Id, masteryAmount),
                TimeSpan.FromMinutes(exploreTime));

            // выводим собирательские ресурсы этой локации
            var resourcesString = gatherings.Aggregate(string.Empty, (current, gathering) =>
                current + $"{emotes.GetEmoteOrBlank(gathering.Name)} {_local.Localize(gathering.Name)}, ");
            var embed = new EmbedBuilder()
                .WithAuthor(Location.ExploreCastle.Localize())
                // баннер исследования сада
                .WithImageUrl(await _imageService.GetImageUrl(Image.ExploreCastle))
                // подверждаем что исследование начато
                .WithDescription(
                    IzumiReplyMessage.ExploreCastleBegin.Parse(Location.Garden.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // ожидаемые награды
                .AddField(IzumiReplyMessage.ExploreRewardFieldName.Parse(),
                    resourcesString.Remove(resourcesString.Length - 2))
                // длительность
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    exploreTime.Minutes().Humanize(2, new CultureInfo("ru-RU")));

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
