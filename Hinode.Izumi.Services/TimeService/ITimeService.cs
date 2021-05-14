using System;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.TimeService
{
    public interface ITimeService
    {
        /// <summary>
        /// Возвращает локализированную строку с информацией о том, сколько осталось времени до указанной даты.
        /// </summary>
        /// <param name="dateTime">Дата.</param>
        /// <returns>Локализированная строка с информацией о том, сколько осталось времени до указанной даты.</returns>
        public string TimeLeft(DateTimeOffset dateTime);

        /// <summary>
        /// Возвращает текущее время суток.
        /// </summary>
        /// <returns>Время суток.</returns>
        public TimesDay GetCurrentTimesDay();
    }
}
