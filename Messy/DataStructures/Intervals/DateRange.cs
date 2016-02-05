using System;
using System.Collections.Generic;
using Messy.Extensions;

namespace Messy.DataStructures.Intervals
{
    /// <summary>
    ///     Интервал дат в днях
    /// </summary>
    public class DateRange : Range<DateTimeOffset>
    {
        public DateRange(DateTimeOffset start, DateTimeOffset end) : base(start.WithoutTime(), end.WithoutTime())
        {
        }

        public DateRange()
        {

        }

        /// <summary>
        ///     Получить набор дней, входящих в интервал
        /// </summary>
        public DateTimeOffset[] GetDays()
        {
            var dates = new List<DateTimeOffset>();
            for (var date = Start; date <= End; date = date.AddDays(1))
            {
                dates.Add(date);
            }
            return dates.ToArray();
        }
    }
}