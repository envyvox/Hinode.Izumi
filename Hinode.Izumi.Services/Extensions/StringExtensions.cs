using System.Text.RegularExpressions;

namespace Hinode.Izumi.Services.Extensions
{
    public static class StringExtensions
    {
        public static bool CheckValid(string name) =>
            Regex.IsMatch(name, @"^[A-ZЁА-Я]{1}[a-zёа-я\s]*$") &&
            name.Length is > 2 and < 17;
    }
}
