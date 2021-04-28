using System.Text.RegularExpressions;

namespace Hinode.Izumi.Services.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Проверяет строку с названием на валидность игрового мира.
        /// </summary>
        /// <param name="name">Название.</param>
        /// <returns>True если валидна, false если нет.</returns>
        public static bool CheckValid(string name) =>
            Regex.IsMatch(name, @"^[A-ZЁА-Я]{1}[a-zёа-я\s]*$") &&
            name.Length > 2 &&
            name.Length < 17;
    }
}
