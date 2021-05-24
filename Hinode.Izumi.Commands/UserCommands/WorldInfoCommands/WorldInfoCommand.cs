using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Hinode.Izumi.Services.TimeService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.WorldInfoCommands
{
    [CommandCategory(CommandCategory.WorldInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class WorldInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly TimeZoneInfo _timeZoneInfo;

        public WorldInfoCommand(IMediator mediator, TimeZoneInfo timeZoneInfo)
        {
            _mediator = mediator;
            _timeZoneInfo = timeZoneInfo;
        }

        [Command("мир"), Alias("world")]
        [Summary("Посмотреть текущее состояние мира")]
        public async Task WorldInfoTask()
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущее время
            var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, _timeZoneInfo);
            // получаем текущее время суток
            var timesDay = await _mediator.Send(new GetCurrentTimesDayQuery());
            // получаем погоду сегодня
            var weatherToday = (Weather) await _mediator.Send(new GetPropertyValueQuery(Property.WeatherToday));
            // получаем погоду завтра
            var weatherTomorrow = (Weather) await _mediator.Send(new GetPropertyValueQuery(Property.WeatherTomorrow));
            // получаем текущий сезон
            var season = (Season) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentSeason));
            // получаем текущее последствие вторжения босса
            var bossDebuff = (BossDebuff) await _mediator.Send(new GetPropertyValueQuery(Property.BossDebuff));

            var embed = new EmbedBuilder()
                // баннер состояние мира
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.WorldInfo)))
                // изображение текущей погоды
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(weatherToday == Weather.Clear
                    ? Image.WeatherClear
                    : Image.WeatherRain)))
                // текущее время дня и на что оно влияет
                .AddField(IzumiReplyMessage.WorldInfoTimeFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoTimeFieldDesc.Parse(
                        timeNow.Hour, timeNow.Minute, timesDay.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // погода сегодня и на что она влияет
                .AddField(IzumiReplyMessage.WorldInfoWeatherTodayFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoWeatherTodayFieldDesc.Parse(
                        weatherToday.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // погода на завтра
                .AddField(IzumiReplyMessage.WorldInfoWeatherTomorrowFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoWeatherTomorrowFieldDesc.Parse(
                        weatherTomorrow.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // текущий сезон и на что он влияет
                .AddField(IzumiReplyMessage.WorldInfoSeasonFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoSeasonFieldDesc.Parse(
                        season.Localize()) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // состояние мира после вторждения босса (информация о дебаффе)
                .AddField(IzumiReplyMessage.WorldInfoDebuffFieldName.Parse(emotes.GetEmoteOrBlank("List")),
                    IzumiReplyMessage.WorldInfoDebuffFieldDesc.Parse(
                        bossDebuff == BossDebuff.None
                            ? bossDebuff.Localize()
                            : IzumiReplyMessage.BossDebuffActive.Parse(
                                  bossDebuff.Location().Localize(true)) +
                              bossDebuff.Localize()));

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            // проверяем нужно ли продвинуть прогресс обучения пользователю
            await _mediator.Send(new CheckUserTutorialStepCommand((long) Context.User.Id, TutorialStep.CheckWorldInfo));
            await Task.CompletedTask;
        }
    }
}
