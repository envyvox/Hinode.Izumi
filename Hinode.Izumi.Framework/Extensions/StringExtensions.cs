using System.Text.RegularExpressions;

namespace Hinode.Izumi.Framework.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var startUnderscores = Regex.Match(input, @"^_+", RegexOptions.Compiled);
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
