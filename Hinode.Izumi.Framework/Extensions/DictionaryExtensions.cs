using System.Collections.Generic;
using System.Linq;

namespace Hinode.Izumi.Framework.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Возвращает максимальное доступное значение из библиотеки.
        /// </summary>
        /// <param name="dictionary">Библиотека.</param>
        /// <param name="value">Значение, по которому будет подбираться ключ.</param>
        /// <returns>Максимальное доступное значение.</returns>
        public static long MaxValue(this Dictionary<long, long> dictionary, long value) =>
            dictionary[dictionary.Keys.Where(x => x <= value).Max()];

        /// <summary>
        /// Возвращает максимальное доступное значение из библиотеки.
        /// </summary>
        /// <param name="dictionary">Библиотека.</param>
        /// <param name="value">Значение, по которому будет подбираться ключ.</param>
        /// <returns>Максимальное доступное значение.</returns>
        public static double MaxValue(this Dictionary<long, double> dictionary, long value) =>
            dictionary[dictionary.Keys.Where(x => x <= value).Max()];

        /// <summary>
        /// Возвращает максимальное доступное значение из библиотеки.
        /// </summary>
        /// <param name="dictionary">Библиотека.</param>
        /// <param name="value">Значение, по которому будет подбираться ключ.</param>
        /// <returns>Максимальное доступное значение.</returns>
        public static long MaxValue(this Dictionary<double, long> dictionary, double value) =>
            dictionary[dictionary.Keys.Where(x => x <= value).Max()];

        /// <summary>
        /// Возвращает максимальное доступное значение из библиотеки.
        /// </summary>
        /// <param name="dictionary">Библиотека.</param>
        /// <param name="value">Значение, по которому будет подбираться ключ.</param>
        /// <returns>Максимальное доступное значение.</returns>
        public static double MaxValue(this Dictionary<double, double> dictionary, double value) =>
            dictionary[dictionary.Keys.Where(x => x <= value).Max()];
    }
}
