using System.Diagnostics;
using Advent.Common.Extensions;
using Advent.Common.Interfaces;
using Advent.Common.Models;

namespace Advent2021.Solvers;

public class Day05 : ISolver
{
    public ISolution Solve(string input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var rows = input.Split(Environment.NewLine);
        var lines = rows.ToLines()
            .Where(r => r.IsStraight);
        var partA = lines
            .SelectMany(l => GetAllPointsOnStraightLine(l.Start, l.End))
            .GroupBy(p => new {p.X, p.Y})
            .Count(g => g.Count() > 1);

        Console.WriteLine($"Took: {stopwatch.Elapsed.TotalMilliseconds}");
        return new Solution(partA.ToString(), null!);
    }

    private IEnumerable<Point> GetAllPointsOnStraightLine(Point start, Point end)
    {
        int min;
        int max;
        if (start.X == end.X)
        {
            min = Math.Min(start.Y, end.Y);
            max = Math.Max(start.Y, end.Y);
        }
        else
        {
            min = Math.Min(start.X, end.X);
            max = Math.Max(start.X, end.X);
        }
        return start.X == end.X
            ? Enumerable.Range(min, max - min + 1)
                .Select(r => new Point {X = start.X, Y = r})
            : Enumerable.Range(min, max - min + 1)
                .Select(r => new Point {X = r, Y = start.Y});
    }
}

public class Point
{
    public int X { get; init; }
    public int Y { get; init; }

    protected bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;

        return obj.GetType() == GetType() && Equals((Point) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"{X},{Y}";
    }
}

public class Line
{
    public Point Start { get; init; } = null!;
    public Point End { get; init; } = null!;

    public bool IsStraight => Start.X == End.X || Start.Y == End.Y;
    
    public override string ToString()
    {
        return $"{Start} -> {End}";
    }
}

public static partial class EnumerableExtensions
{
    public static IEnumerable<Line> ToLines(this IEnumerable<string> rows)
    {
        foreach (var row in rows)
        {
            var pointStrings = row.Split(" -> ");
            var start = pointStrings[0].Split(',');
            var startPoint = new Point {X = start[0].ToInt(), Y = start[1].ToInt()};
            var end = pointStrings[1].Split(',');
            var endPoint = new Point {X = end[0].ToInt(), Y = end[1].ToInt()};

            yield return new Line {Start = startPoint, End = endPoint};
        }
    }
}