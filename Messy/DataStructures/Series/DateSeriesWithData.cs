namespace Messy.DataStructures.Series
{
    /// <summary>
    ///     Date series with additional information
    /// </summary>
    /// <typeparam name="TAdditionalData">Type of additional information</typeparam>
    /// <typeparam name="TPoint">Type of points</typeparam>
    public class DateSeriesWithData<TAdditionalData, TPoint>
    {
        /// <summary>
        ///     Additional information related to the series
        /// </summary>
        public TAdditionalData Data { get; private set; }

        public DateSeries<TPoint> Series { get; private set; }

        public DateSeriesWithData(TAdditionalData data, DateSeries<TPoint> series)
        {
            Data = data;
            Series = series;
        }
    }
}