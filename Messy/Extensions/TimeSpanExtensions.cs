using System;

namespace Messy.Extensions
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan SubstractWithinDay(this TimeSpan timeSpan, TimeSpan timeSpanToSubstract)
        {
            var sub = timeSpan.Subtract(timeSpanToSubstract);
            if (sub < TimeSpan.Zero)
            {
                return TimeSpan.FromHours(24).Add(sub);
            }
            return sub;
        }
    }
}