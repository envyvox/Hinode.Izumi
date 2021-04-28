using System.Collections.Generic;
using Hinode.Izumi.Framework.Extensions;
using Hinode.Izumi.Services.RpgServices.PropertyService.Models;

namespace Hinode.Izumi.Services.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Возвращает максимальное доступное свойство мастерства.
        /// </summary>
        /// <param name="properties">Свойства мастерства.</param>
        /// <param name="mastery">Количество мастерства у пользователя.</param>
        /// <returns>Максимальное доступное свойство мастерства.</returns>
        public static long MasteryMaxValue(this MasteryPropertyModel properties, long mastery) =>
            new Dictionary<long, long>
            {
                {0, properties.Mastery0},
                {50, properties.Mastery50},
                {100, properties.Mastery100},
                {150, properties.Mastery150},
                {200, properties.Mastery200},
                {250, properties.Mastery250}
            }.MaxValue(mastery);

        /// <summary>
        /// Возвращает максимальное доступное свойство мастерства.
        /// </summary>
        /// <param name="properties">Свойства изготавливаемого предмета.</param>
        /// <param name="mastery">Количество мастерства у пользователя.</param>
        /// <returns>Максимальное доступное свойство.</returns>
        public static long MasteryMaxValue(this CraftingPropertyModel properties, long mastery) =>
            new Dictionary<long, long>
            {
                {0, properties.Mastery0},
                {50, properties.Mastery50},
                {100, properties.Mastery100},
                {150, properties.Mastery150},
                {200, properties.Mastery200},
                {250, properties.Mastery250}
            }.MaxValue(mastery);

        /// <summary>
        /// Возвращает максимальное доступное свойство мастерства.
        /// </summary>
        /// <param name="properties">Свойства собирательского ресурса.</param>
        /// <param name="mastery">Количество мастерства у пользователя.</param>
        /// <returns>Максимальное доступное свойство.</returns>
        public static long MasteryMaxValue(this GatheringPropertyModel properties, long mastery) =>
            new Dictionary<long, long>
            {
                {0, properties.Mastery0},
                {50, properties.Mastery50},
                {100, properties.Mastery100},
                {150, properties.Mastery150},
                {200, properties.Mastery200},
                {250, properties.Mastery250}
            }.MaxValue(mastery);

        /// <summary>
        /// Возвращает максимальное доступное свойство мастерства.
        /// </summary>
        /// <param name="properties">Свойства алкоголя.</param>
        /// <param name="mastery">Количество мастерства у пользователя.</param>
        /// <returns>Максимальное доступное свойство.</returns>
        public static long MasteryMaxValue(this AlcoholPropertyModel properties, long mastery) =>
            new Dictionary<long, long>
            {
                {0, properties.Mastery0},
                {50, properties.Mastery50},
                {100, properties.Mastery100},
                {150, properties.Mastery150},
                {200, properties.Mastery200},
                {250, properties.Mastery250}
            }.MaxValue(mastery);

        /// <summary>
        /// Возвращает максимальное доступное свойство мастерства.
        /// </summary>
        /// <param name="properties">Свойства мастерства.</param>
        /// <param name="mastery">Количество мастерства у пользователя.</param>
        /// <returns>Максимальное доступное свойство мастерства.</returns>
        public static double MasteryXpMaxValue(this MasteryXpPropertyModel properties, long mastery) =>
            new Dictionary<long, double>
            {
                {0, properties.Mastery0},
                {50, properties.Mastery50},
                {100, properties.Mastery100},
                {150, properties.Mastery150},
                {200, properties.Mastery200},
                {250, properties.Mastery250}
            }.MaxValue(mastery);
    }
}
