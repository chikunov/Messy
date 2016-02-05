using System.Configuration;

namespace Messy
{
    public static class ApplicationConfig
    {
        public static string GetConfigValue(string key, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }
}