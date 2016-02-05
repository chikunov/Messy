namespace Messy.DataStructures.Series
{
    /// <summary>
    ///     Подневный временной ряд со связанной информацией
    /// </summary>
    /// <typeparam name="TAdditionalData">Тип связанной информации</typeparam>
    /// <typeparam name="TPoint">Тип точек временного ряда</typeparam>
    public class DateSeriesWithData<TAdditionalData, TPoint>
    {
        /// <summary>
        ///     Дополнительная информация, связанная с временным рядом
        /// </summary>
        public TAdditionalData Data { get; private set; }

        /// <summary>
        ///     Подневный временной ряд
        /// </summary>
        public DateSeries<TPoint> Series { get; private set; }

        public DateSeriesWithData(TAdditionalData data, DateSeries<TPoint> series)
        {
            Data = data;
            Series = series;
        }
    }
}