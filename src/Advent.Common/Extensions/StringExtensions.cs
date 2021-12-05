namespace Advent.Common.Extensions;

public static class StringExtensions
{
    public static int ToInt(this string input)
    {
        var result = 0;
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var c in input) result = result * 10 + (c - '0');
        return result;
    }
}