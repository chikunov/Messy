using System;

namespace Messy.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        private static readonly DateTime OneOldMonday = new DateTime(1900, 01, 01);

        public static DateTimeOffset WithoutTime(this DateTimeOffset dateTimeOffset)
        {
            return new DateTimeOffset(dateTimeOffset.Date, dateTimeOffset.Offset);
        }

        public static DateTimeOffset BeginOfMonth(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, dt.Month, 1, 0, 0, 0, dt.Offset);
        }

        public static DateTimeOffset BeginOfWeek(this DateTimeOffset dt)
        {
            var oldLocalMonday = new DateTimeOffset(OneOldMonday, dt.Offset);
            return dt.AddDays(-1 * ((dt - oldLocalMonday).Days % 7));
        }

        public static DateTimeOffset Min(DateTimeOffset d1, DateTimeOffset d2)
        {
            return d1 <= d2 ? d1 : d2;
        }

        public static DateTimeOffset Max(DateTimeOffset d1, DateTimeOffset d2)
        {
            return d1 >= d2 ? d1 : d2;
        }
    }
}