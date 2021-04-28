using System.Collections.Generic;
using Hinode.Izumi.Services.EmoteService.Models;

namespace Hinode.Izumi.Services.EmoteService.Impl
{
    public static class EmoteServiceExtensions
    {
        /// <summary>
        /// Получает код иконки по названию.
        /// В случае если иконки с таким названием в словаре нет - возвращает код иконки Blank.
        /// </summary>
        /// <param name="dictionary">Словарь иконок.</param>
        /// <param name="key">Название иконки.</param>
        /// <returns>Код иконки.</returns>
        public static string GetEmoteOrBlank(this Dictionary<string, EmoteModel> dictionary, string key)
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
    }
}
