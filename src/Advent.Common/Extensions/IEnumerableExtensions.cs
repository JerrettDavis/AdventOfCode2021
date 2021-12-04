namespace Advent.Common.Extensions;

// ReSharper disable once InconsistentNaming
public static class IEnumerableExtensions
{
    public static IEnumerable<(T? PrevItem, T? CurrentItem, T? NextItem)> 
        SlidingWindow<T>(
            this IEnumerable<T?> source, 
            T? emptyValue = default)
    {
        var prevItem = emptyValue;
        var currentItem = emptyValue;
        foreach (var nextItem in source)
        {
            yield return (prevItem, currentItem, nextItem);
            prevItem = currentItem;
            currentItem = nextItem;
        }
    }

    public static IEnumerable<bool> GreaterThanPrevious<T>(
        this IEnumerable<T?> source,
        T? emptyValue = default) where T : IComparable
    {
        var prevItem = emptyValue;
        foreach (var currentItem in source)
        {
            yield return prevItem?.CompareTo(currentItem) < 0;
            prevItem = currentItem;
        } 
            
    }
}