using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.BackgroundJobs.ExploreJob;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ExploreCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    [IzumiRequireLocation(Location.Seaport), IzumiRequireNoDebuff(BossDebuff.SeaportStop)]
    public class FishingCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocationService _locationService;
        private readonly IMasteryService _masteryService;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly ICalculationService _calc;
        private readonly IPropertyService _propertyService;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly IBuildingService _buildingService;

        public FishingCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocationService locationService, IMasteryService masteryService, IImageService imageService,
            IUserService userService, ICalculationService calc, IPropertyService propertyService,
            TimeZoneInfo timeZoneInfo, IBuildingService buildingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _locationService = locationService;
            _masteryService = masteryService;
            _imageService = imageService;
            _userService = userService;
            _calc = calc;
            _propertyService = propertyService;
            _timeZoneInfo = timeZoneInfo;
            _buildingService = buildingService;
        }

        [Command("рыбачить"), Alias("fishing")]
        public async Task FishingTask()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем текущее время
            var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo);
            // получаем пользователя из базы
            var user = await _userService.GetUser((long) Context.User.Id);
            // получаем мастерство рыбалки пользователя
            var userMastery = await _masteryService.GetUserMastery((long) Context.User.Id, Mastery.Fishing);

            // проверяем есть ли у пользователя рыбацкая лодка
            var hasFishingBoat = await _buildingService.CheckBuildingInUser(
                (long) Context.User.Id, Building.FishingBoat);
            // определяем длительность рыбалки
            var fishingTime = await _calc.FishingTime(user.Energy, hasFishingBoat);

            // обновляем текущую локацию пользователя
            await _locationService.UpdateUserLocation((long) Context.User.Id, Location.Fishing);
            // добавляем информацию о перемещении
            await _locationService.AddUserMovement((long) Context.User.Id,
                Location.Fishing, Location.Seaport, timeNow.AddMinutes(fishingTime));
            // отнимаем энергию у пользователя
            await _userService.RemoveEnergyFromUser((long) Context.User.Id,
                await _propertyService.GetPropertyValue(Property.EnergyCostExplore));

            // запускаем джобу для окончания рыбалки
            BackgroundJob.Schedule<IExploreJob>(x =>
                    x.CompleteFishing((long) Context.User.Id, (long) Math.Floor(userMastery.Amount)),
                TimeSpan.FromMinutes(fishingTime));

            var embed = new EmbedBuilder()
                .WithAuthor(Location.Fishing.Localize())
                // баннер рыбалки
                .WithImageUrl(await _imageService.GetImageUrl(Image.Fishing))
                // подтверждаем успешное начало рыбалки
                .WithDescription(
                    IzumiReplyMessage.FishingBegin.Parse(Location.Seaport.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // ожидаемая награда
                .AddField(IzumiReplyMessage.ExploreRewardFieldName.Parse(),
                    IzumiReplyMessage.ExploreRewardFishingFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("SlimejackBW")), true)
                // длительность
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    fishingTime.Minutes().Humanize(2, new CultureInfo("ru-RU")), true);

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
