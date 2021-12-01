using System;
using System.Collections.Generic;

namespace AdventRunner.Common.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T? PrevItem, T? CurrentItem, T? NextItem)>
            SlidingWindow<T>(this IEnumerable<T?> source, T? emptyValue = default)
        {
            using var iter = source.GetEnumerator();
            if (!iter.MoveNext())
                yield break;
            var prevItem = emptyValue;
            var currentItem = iter.Current;
            while (iter.MoveNext())
            {
                var nextItem = iter.Current;
                yield return (prevItem, currentItem, nextItem);
                prevItem = currentItem;
                currentItem = nextItem;
            }
            yield return (prevItem, currentItem, emptyValue);
        }

        public static IEnumerable<bool> GreaterThanPrevious<T>(
            this IEnumerable<T?> source,
            T? emptyValue = default) where T : IComparable
        {
            using var iterator = source.GetEnumerator();
            if (!iterator.MoveNext())
                yield break;
            var prevItem = emptyValue;
            var currentItem = iterator.Current;
            while (iterator.MoveNext())
            {
                var nextItem = iterator.Current;
                yield return prevItem?.CompareTo(currentItem) < 0;
                prevItem = currentItem;
                currentItem = nextItem;
            }
            yield return prevItem?.CompareTo(currentItem) < 0;
        }
    }
}