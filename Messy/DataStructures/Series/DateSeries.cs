using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Messy.DataStructures.Intervals;
using Messy.Extensions;

namespace Messy.DataStructures.Series
{
    /// <summary>
    ///     Series of points with values per day
    /// </summary>
    /// <typeparam name="TPoint">Type of point</typeparam>
    public class DateSeries<TPoint> : IEnumerable<KeyValuePair<DateTimeOffset, TPoint>>
    {
        public TPoint this[DateTimeOffset date]
        {
            get { return _points[date]; }
            set { _points[date] = value; }
        }

        private readonly Dictionary<DateTimeOffset, TPoint> _points;

        public DateSeries()
        {
            _points = new Dictionary<DateTimeOffset, TPoint>();
        }

        public DateSeries(Dictionary<DateTimeOffset, TPoint> points)
        {
            _points = points.ToDictionary(d => d.Key.WithoutTime(), d => d.Value);
        }

        public DateSeries(IEnumerable<KeyValuePair<DateTimeOffset, TPoint>> points)
        {
            _points = points.ToDictionary(p => p.Key.WithoutTime(), p => p.Value);
        }

        public IEnumerator<KeyValuePair<DateTimeOffset, TPoint>> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Translate to another DateSeries using converter
        /// </summary>
        /// <typeparam name="TOther">New series point type</typeparam>
        /// <param name="converter">Converter function for translation</param>
        public DateSeries<TOther> ToDateSeries<TOther>(Func<DateTimeOffset, TPoint, TOther> converter)
        {
            var result = new DateSeries<TOther>();
            foreach (var point in _points)
            {
                result.AddPoint(point.Key, converter(point.Key, point.Value));
            }

            return result;
        }


        /// <summary>
        ///     Translate to another DateSeries using converter
        /// </summary>
        /// <typeparam name="TOther">New series point type</typeparam>
        /// <param name="converter">Converter function for translation</param>
        public DateSeries<TOther> ToDateSeries<TOther>(Func<TPoint, TOther> converter)
        {
            var result = new DateSeries<TOther>();
            foreach (var point in _points)
            {
                result.AddPoint(point.Key, converter(point.Value));
            }

            return result;
        }

        /// <summary>
        ///     Get gapless series on interval using default value of gaps
        /// </summary>
        /// <param name="dateRange">Date interval</param>
        /// <param name="defaultValue">Default value for gaps</param>
        public DateSeries<TPoint> GetGaplessSeries(DateRange dateRange, TPoint defaultValue)
        {
            var dateSeries = new DateSeries<TPoint>();
            for (var date = dateRange.Start; date <= dateRange.End; date = date.AddDays(1))
            {
                TPoint point;
                dateSeries.AddPoint(date, _points.TryGetValue(date, out point) ? point : defaultValue);
            }

            return dateSeries;
        }

        /// <summary>
        ///     Get start date of the series
        /// </summary>
        public DateTimeOffset GetMinDate()
        {
            return _points.Keys.Min();
        }

        /// <summary>
        ///     Get end date of the series
        /// </summary>
        public DateTimeOffset GetMaxDate()
        {
            return _points.Keys.Max();
        }

        /// <summary>
        ///     Get series interval
        /// </summary>
        public DateRange GetDateRange()
        {
            return new DateRange(GetMinDate(), GetMaxDate());
        }

        /// <summary>
        ///     Get dates with values in series
        /// </summary>
        public DateTimeOffset[] GetDates()
        {
            return _points.Keys.ToArray();
        }

        /// <summary>
        ///     Add point to series
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="point">Point's value</param>
        public void AddPoint(DateTimeOffset date, TPoint point)
        {
            date = date.WithoutTime();

            if (_points.ContainsKey(date))
            {
                throw new Exception();
            }

            _points.Add(date, point);
        }

        /// <summary>
        ///     Get point's value on date
        /// </summary>
        /// <param name="date">Date of the series</param>
        public TPoint GetPoint(DateTimeOffset date)
        {
            date = date.WithoutTime();

            TPoint point;
            if (!_points.TryGetValue(date, out point))
            {
                throw new Exception();
            }

            return point;
        }

        /// <summary>
        ///     Get point's value on date if it exists
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="point">Point's value</param>
        public bool TryGetPoint(DateTimeOffset date, out TPoint point)
        {
            date = date.WithoutTime();

            return _points.TryGetValue(date, out point);
        }

        /// <summary>
        ///     Содержит ли ряд значение на дату
        /// </summary>
        /// <param name="date">Дата</param>
        public bool ContainsPointOnDate(DateTimeOffset date)
        {
            date = date.WithoutTime();

            return _points.ContainsKey(date);
        }

        /// <summary>
        ///     Есть ли у данных рядов общие точки
        /// </summary>
        /// <param name="otherDateSeries">Ряд для сравнения</param>
        public bool HasCommonPoints(DateSeries<TPoint> otherDateSeries)
        {
            var dates = otherDateSeries.GetDates();
            return _points.Keys.Any(p => dates.Contains(p));
        }

        /// <summary>
        ///     Объединить два подневных ряда с помощью функции объединения, применяемого к каждой точке
        /// </summary>
        /// <param name="other">Другой ряд</param>
        /// <param name="merger">Функция объединения</param>
        public DateSeries<TPoint> Merge(DateSeries<TPoint> other, Func<TPoint, TPoint, TPoint> merger)
        {
            var seriesInterval = GetDateRange();
            var otherInterval = other.GetDateRange();
            var startDate = seriesInterval.Start < otherInterval.Start ? seriesInterval.Start : otherInterval.Start;
            var endDate = seriesInterval.End > otherInterval.End ? seriesInterval.End : otherInterval.End;
            var resultSeries = new DateSeries<TPoint>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                TPoint seriesPoint;
                TPoint otherPoint;
                TryGetPoint(date, out seriesPoint);
                other.TryGetPoint(date, out otherPoint);

                resultSeries.AddPoint(date, merger(seriesPoint, otherPoint));
            }

            return resultSeries;
        }
    }
}