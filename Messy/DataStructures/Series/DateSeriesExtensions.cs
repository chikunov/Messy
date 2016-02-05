using System;
using System.Collections.Generic;

namespace Messy.DataStructures.Series
{
    public static class DateSeriesExtensions
    {
        public static DateSeries<TPoint> ToDateSeries<TPoint>(this IDictionary<DateTimeOffset, TPoint> dictionary)
        {
            return new DateSeries<TPoint>(dictionary);
        }
    }
}
