using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Событие.
    /// </summary>
    public enum Event
    {
        None = 0,
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public static class EventHelper
    {
        /// <summary>
        /// Возвращает локализированное название события.
        /// </summary>
        /// <param name="event">Событие.</param>
        /// <returns>Локализированное название события.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Localize(this Event @event) => @event switch
        {
            Event.None => "...",
            Event.January => "Январь месяц",
            Event.February => "Февраль месяц",
            Event.March => "Март месяц",
            Event.April => "Апрель месяц",
            Event.May => "Май месяц",
            Event.June => "Июнь месяц",
            Event.July => "Июль месяц",
            Event.August => "Август месяц",
            Event.September => "Сентабрь месяц",
            Event.October => "Октабрь месяц",
            Event.November => "Ноябрь месяц",
            Event.December => "Декабрь месяц",
            _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null)
        };
    }
}
