using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Hinode.Izumi.Services.TimeService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.WorldInfoCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class WorldInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IPropertyService _propertyService;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ITrainingService _trainingService;
        private readonly ITimeService _timeService;
        private readonly IImageService _imageService;

        public WorldInfoCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IPropertyService propertyService, TimeZoneInfo timeZoneInfo, ITrainingService trainingService,
            ITimeService timeService, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _propertyService = propertyService;
            _timeZoneInfo = timeZoneInfo;
            _trainingService = trainingService;
            _timeService = timeService;
            _imageService = imageService;
        }

        [Command("мир"), Alias("world")]
        public async Task WorldInfoTask()
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем текущее время
            var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, _timeZoneInfo);
            // получаем текущее время суток
            var timesDay = _timeService.GetCurrentTimesDay();
            // получаем погоду сегодня
            var weatherToday = (Weather) await _propertyService.GetPropertyValue(Property.WeatherToday);
            // получаем погоду завтра
            var weatherTomorrow = (Weather) await _propertyService.GetPropertyValue(Property.WeatherTomorrow);
            // получаем текущий сезон
            var season = (Season) await _propertyService.GetPropertyValue(Property.CurrentSeason);
            // получаем текущее последствие вторжения босса
            var bossDebuff = (BossDebuff) await _propertyService.GetPropertyValue(Property.BossDebuff);

            var embed = new EmbedBuilder()
                // баннер состояние мира
                .WithImageUrl(await _imageService.GetImageUrl(Image.WorldInfo))
                // изображение текущей погоды
                .WithThumbnailUrl(await _imageService.GetImageUrl(weatherToday == Weather.Clear
                    ? Image.WeatherClear
                    : Image.WeatherRain))
                // текущее время дня и на что оно влияет
                .AddField(IzumiReplyMessage.WorldInfoTimeFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoTimeFieldDesc.Parse(
                        timeNow.Hour, timeNow.Minute, timesDay.Localize()))
                // погода сегодня и на что она влияет
                .AddField(IzumiReplyMessage.WorldInfoWeatherTodayFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoWeatherTodayFieldDesc.Parse(
                        weatherToday.Localize()))
                // погода на завтра
                .AddField(IzumiReplyMessage.WorldInfoWeatherTomorrowFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoWeatherTomorrowFieldDesc.Parse(
                        weatherTomorrow.Localize()))
                // текущий сезон и на что он влияет
                .AddField(IzumiReplyMessage.WorldInfoSeasonFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoSeasonFieldDesc.Parse(
                        season.Localize()))
                // состояние мира после вторждения босса (информация о дебаффе)
                .AddField(IzumiReplyMessage.WorldInfoDebuffFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoDebuffFieldDesc.Parse(
                        bossDebuff == BossDebuff.None
                            ? bossDebuff.Localize()
                            : IzumiReplyMessage.BossDebuffActive.Parse(
                                  bossDebuff.Location().Localize(true)) +
                              bossDebuff.Localize()))
                // состояние мире (заделка на будущую система доната)
                .AddField(IzumiReplyMessage.WorldInfoStateFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoStateFieldDesc.Parse());

            await _discordEmbedService.SendEmbed(Context.User, embed);
            // проверяем нужно ли продвинуть прогресс обучения пользователю
            await _trainingService.CheckStep((long) Context.User.Id, TrainingStep.CheckWorldInfo);
            await Task.CompletedTask;
        }
    }
}
