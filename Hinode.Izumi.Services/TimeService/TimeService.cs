using System;
using System.Globalization;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Humanizer;
using CoordinateSharp;

namespace Hinode.Izumi.Services.TimeService
{
    [InjectableService]
    public class TimeService : ITimeService
    {
        private readonly TimeZoneInfo _timeZoneInfo;

        public TimeService(TimeZoneInfo timeZoneInfo)
        {
            _timeZoneInfo = timeZoneInfo;
        }

        public string TimeLeft(DateTimeOffset dateTime) =>
            $@"заканчивается через {TimeSpan
                .FromMinutes(dateTime
                    .Subtract(TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo))
                    .TotalMinutes)
                .Humanize(1, new CultureInfo("ru-RU"))}";

        public TimesDay GetCurrentTimesDay()
        {
            var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo);
            var c = new Coordinate(55.915379, 37.824598, timeNow);

            return timeNow > c.CelestialInfo.SunRise &&
                   timeNow < c.CelestialInfo.SunSet
                ? TimesDay.Day
                : TimesDay.Night;
        }
    }
}
