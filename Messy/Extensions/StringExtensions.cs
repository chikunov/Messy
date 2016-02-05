using System.Globalization;

namespace Messy.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsAnyCharacters(this string name)
        {
            return !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name);
        }

        public static string TrimAndMakeUppercase(this string name, CultureInfo cultureInfo)
        {
            return name.Trim().ToUpper(cultureInfo);
        }
    }
}