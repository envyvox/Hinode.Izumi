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
        public string TimeLeft(DateTime dateTime);

        /// <summary>
        /// Проверяет находится ли текущее время между указанными двумя.
        /// </summary>
        /// <param name="begin">Время начала.</param>
        /// <param name="end">Время конца.</param>
        /// <returns>True если да, false если нет.</returns>
        public bool TimeBetween(TimeSpan begin, TimeSpan end);

        /// <summary>
        /// Возвращает текущее время суток.
        /// </summary>
        /// <returns>Время суток.</returns>
        public TimesDay GetCurrentTimesDay();
    }
}
