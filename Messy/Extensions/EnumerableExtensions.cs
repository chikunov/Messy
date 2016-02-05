using System.Collections.Generic;
using System.Linq;

namespace Messy.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> source)
        {
            return source ?? Enumerable.Empty<TSource>();
        }

        public static bool NullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }
    }
}