using System;

namespace Messy.DataStructures.Intervals
{
    /// <summary>
    ///     Interval of points of the type T
    /// </summary>
    /// <typeparam name="TPoint">Point type</typeparam>
    public class Range<TPoint> where TPoint : struct, IComparable
    {
        /// <summary>
        ///     Left bound of the interval
        /// </summary>
        public TPoint End { get; }

        /// <summary>
        ///     Right bound of the interval
        /// </summary>
        public TPoint Start { get; }

        public Range()
        {

        }

        public Range(TPoint start, TPoint end)
        {
            if (start.CompareTo(end) > 0)
            {
                Start = end;
                End = start;
            }
            else
            {
                Start = start;
                End = end;
            }
        }

        /// <summary>
        ///     Is interval empty
        /// </summary>
        public bool IsEmpty()
        {
            return Start.CompareTo(End) == 0;
        }

        /// <summary>
        ///     Is overlapped with the given interval
        /// </summary>
        /// <param name="secondRange">Interval</param>
        public bool IsOverlapped(Range<TPoint> secondRange)
        {
            return !(secondRange.End.CompareTo(Start) < 0 || secondRange.Start.CompareTo(End) > 0);
        }

        /// <summary>
        ///     Does interval contain the given point
        /// </summary>
        /// <param name="value">Point of the interval</param>
        public bool Contains(TPoint value)
        {
            return value.CompareTo(Start) >= 0 && value.CompareTo(End) <= 0;
        }
    }
}