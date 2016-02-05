using System;
using System.ComponentModel;

namespace Messy.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            string output = null;
            var type = value.GetType();

            var fieldInfo = type.GetField(value.ToString());
            var attrs = fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), false) as DescriptionAttribute[];
            if (attrs != null && attrs.Length == 1)
            {
                output = attrs[0].Description;
            }

            return output;
        }
    }
}