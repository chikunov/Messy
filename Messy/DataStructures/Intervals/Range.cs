using System;

namespace Messy.DataStructures.Intervals
{
    /// <summary>
    ///     Интервал типа T
    /// </summary>
    /// <typeparam name="TPoint">Тип точки интервала</typeparam>
    public class Range<TPoint> where TPoint : struct, IComparable
    {
        /// <summary>
        ///     Конец интервала
        /// </summary>
        public TPoint End { get; }

        /// <summary>
        ///     Начало интервала
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
        ///     Пустой ли интервал
        /// </summary>
        public bool IsEmpty()
        {
            return Start.CompareTo(End) == 0;
        }

        /// <summary>
        ///     Пересекается ли интервал с данным интервалом
        /// </summary>
        /// <param name="secondRange">Данный интервал</param>
        public bool IsOverlaps(Range<TPoint> secondRange)
        {
            return !(secondRange.End.CompareTo(Start) < 0 || secondRange.Start.CompareTo(End) > 0);
        }

        /// <summary>
        ///     Содержит ли интервал данную точку
        /// </summary>
        /// <param name="value">Точка интервала</param>
        public bool Contains(TPoint value)
        {
            return value.CompareTo(Start) >= 0 && value.CompareTo(End) <= 0;
        }
    }
}