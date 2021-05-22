using System.Collections.Generic;
using Hinode.Izumi.Framework.Extensions;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.GameServices.PropertyService.Records;

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
        public static long MasteryMaxValue(this MasteryPropertyRecord properties, long mastery) =>
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
        public static long MasteryMaxValue(this CraftingPropertyRecord properties, long mastery) =>
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
        public static long MasteryMaxValue(this GatheringPropertyRecord properties, long mastery) =>
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
        public static long MasteryMaxValue(this AlcoholPropertyRecord properties, long mastery) =>
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
        public static double MasteryXpMaxValue(this MasteryXpPropertyRecord properties, long mastery) =>
            new Dictionary<long, double>
            {
                {0, properties.Mastery0},
                {50, properties.Mastery50},
                {100, properties.Mastery100},
                {150, properties.Mastery150},
                {200, properties.Mastery200},
                {250, properties.Mastery250}
            }.MaxValue(mastery);

        /// <summary>
        /// Получает код иконки по названию.
        /// В случае если иконки с таким названием в словаре нет - возвращает код иконки Blank.
        /// </summary>
        /// <param name="dictionary">Словарь иконок.</param>
        /// <param name="key">Название иконки.</param>
        /// <returns>Код иконки.</returns>
        public static string GetEmoteOrBlank(this Dictionary<string, EmoteRecord> dictionary, string key)
        {
            // Ищем в словаре нужную иконку
            return dictionary.TryGetValue(key, out var value)
                // Если такая есть - возвращаем ее код
                ? value.Code
                // Если такой нет - ищем код иконки Blank
                : dictionary.TryGetValue("Blank", out var blankValue)
                    // Если такая есть - возвращаем ее код
                    ? blankValue.Code
                    // Если такой нет - вероятнее всего словарь пустой и нужно вернуть статичное значение
                    : "<:Blank:813150566695174204>";
        }

        /// <summary>
        /// Получает id иконки по названию.
        /// В случае если иконки с таким названием в словаре нет - возвращает id иконки Blank.
        /// </summary>
        /// <param name="dictionary">Словарь иконок.</param>
        /// <param name="key">Название иконки.</param>
        /// <returns>Id иконки.</returns>
        public static string GetEmoteIdOrBlank(this Dictionary<string, EmoteRecord> dictionary, string key)
        {
            // Ищем в словаре нужную иконку
            return dictionary.TryGetValue(key, out var value)
                // Если такая есть - возвращаем ее id как строку
                ? value.Id.ToString()
                // Если такой нет - ищем id иконки Blank
                : dictionary.TryGetValue("Blank", out var blankValue)
                    // Если такая есть - возвращаем ее id как строку
                    ? blankValue.Id.ToString()
                    // Если такой нет - вероятнее всего словарь пустой и нужно вернуть статичное значение
                    : "813150566695174204";
        }
    }
}
