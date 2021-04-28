using System;

namespace Hinode.Izumi.Data.Enums
{
    /// <summary>
    /// Состояние ячейки участка.
    /// </summary>
    public enum FieldState
    {
        Empty = 0, // состояние по-умолчанию
        Planted = 1,
        Watered = 2,
        Completed = 3
    }

    public static class FieldStateHelper
    {
        /// <summary>
        /// Возвращает локализированное название состояния ячейки участка.
        /// </summary>
        /// <param name="state">Состояние ячейки участка.</param>
        /// <returns>Локализированное название состояния ячейки участка.</returns>
        public static string Localize(this FieldState state) => state switch
        {
            FieldState.Empty => "Пустая",
            FieldState.Planted => "Засажена",
            FieldState.Watered => "Полита",
            FieldState.Completed => "Готово к сбору",
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}
