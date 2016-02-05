using System;

namespace Messy.DataStructures.Intervals
{
    public class DateTimeRange : Range<DateTimeOffset>
    {
        public DateTimeRange(DateTimeOffset start, DateTimeOffset end) : base(start, end)
        {
        }
    }
}
