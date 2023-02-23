using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiban
{
    internal static class LinqExtensions
    {
        public static TSource AggregateOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func, TSource defaultValue)
        {
            if (!source.Any())
                return defaultValue;

            return source.Aggregate(func);
        }

        public static void AddOrUpdate<TKey, IValue>(this IDictionary<TKey, IValue> source, TKey key, IValue value)
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }
    }
}
